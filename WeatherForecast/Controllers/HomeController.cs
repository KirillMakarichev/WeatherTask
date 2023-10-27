using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WeatherForecast.Mappers;
using WeatherForecast.Models;
using WeatherForecast.Services.Interfaces;
using WeatherForecast.ViewModels;

namespace WeatherForecast.Controllers;

public class HomeController : Controller
{
    private readonly IWeatherParser _weatherParser;
    private readonly IArchivesRepository _archivesRepository;
    private readonly ICompositeViewEngine _viewEngine;

    public HomeController(IWeatherParser weatherParser, IArchivesRepository archivesRepository,
        ICompositeViewEngine viewEngine)
    {
        _weatherParser = weatherParser;
        _archivesRepository = archivesRepository;
        _viewEngine = viewEngine;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var years = await _archivesRepository.GetAllYearsAsync();
        return View(new ArchivesViewModel()
        {
            Years = years,
        });
    }

    [HttpGet]
    public async Task<IActionResult> FilterData(int? year = null, int? month = null, int take = 10, int skip = 0)
    {
        var filteredData = await _archivesRepository.GetAsync(year, month, take, skip);
        var result = filteredData.Data.Select(WeatherDataMappers.ModelFromDbData).ToList();

        var partialViewResult = PartialView("ArchivesView", result);

        var viewContent = ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);

        var combinedResult = new
        {
            Count = filteredData.Count,
            PartialView = viewContent
        };

        return Json(combinedResult);
    }

    public IActionResult Archives()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFileCollection xlsxFiles)
    {
        var filesStatuses = new List<(string name, FileLoadingStatus)>(xlsxFiles.Count);
        var query = xlsxFiles.AsParallel().Select(file =>
            {
                var readStream = file.OpenReadStream();
                var parsedData = _weatherParser.ParseWeather(readStream);
                readStream.Close();
                filesStatuses.Add((file.FileName, parsedData.Error));
                return parsedData;
            }).Where(x => x.Error == FileLoadingStatus.Ok
                          && x.Response.Any())
            .Select(x => x.Response);

        var data = query.SelectMany(parsedData => parsedData).Select(WeatherDataMappers.DbDataFromModel)
            .Where(x => x != null).ToList();

        var saved = false;
        if (data.Any())
        {
            saved = await _archivesRepository.SaveAsync(data);
        }

        return PartialView("FilesStatus", new FilesLoadingResponse()
        {
            Statuses = filesStatuses.Select(x => (x.name, x.Item2)).ToList(),
            Saved = saved
        });
    }

    private string ConvertViewToString(ControllerContext controllerContext, PartialViewResult pvr,
        ICompositeViewEngine _viewEngine)
    {
        using var writer = new StringWriter();
        var vResult = _viewEngine.FindView(controllerContext, pvr.ViewName, false);
        var viewContext = new ViewContext(controllerContext, vResult.View, pvr.ViewData, pvr.TempData, writer,
            new HtmlHelperOptions());

        vResult.View.RenderAsync(viewContext);

        return writer.GetStringBuilder().ToString();
    }
}