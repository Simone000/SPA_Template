using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

using SpaTemplateStartHere.Areas.HelpPage;
using SpaTemplateStartHere.Areas.HelpPage.ModelDescriptions;

namespace SpaTemplateJavascriptGenerator.Controllers.JavascriptGenerator
{
    [RoutePrefix("api/JavascriptGenerator")]
    public class JavascriptGeneratorController : ApiController
    { 
#if DEBUG
        [HttpGet]
        [Route("GenerateAPI")]
        public IHttpActionResult GenerateAPI()
        {
            string jsMethodsCall = string.Empty;
            foreach (var apiController in ApiControllers)
            {
                string controllerName = apiController.Key.ControllerName;

                //commento col nome del controller
                jsMethodsCall += Environment.NewLine
                    + @"//" + controllerName
                    + Environment.NewLine;

                foreach (var apiMethod in apiController)
                {
                    string relativePath = apiMethod.RelativePath;
                    string httpMethod = apiMethod.HttpMethod.Method;
                    string actionMethod = apiMethod.ActionDescriptor.ActionName;

                    string friendlyID = apiMethod.GetFriendlyId();
                    var apiModel = GlobalConfiguration.Configuration.GetHelpPageApiModel(friendlyID);

                    bool doesReturnJson = apiModel.ResourceDescription
                        .ModelType.Name != "IHttpActionResult" ? true : false;
                    string doesReturnJsonString = doesReturnJson ? "true" : "false";
                    
                    bool isGet = false;
                    if (httpMethod == "GET")
                        isGet = true;
                    else if (httpMethod != "POST")
                        continue;
                    
                    string jsCurrentMethodCall = string.Empty;

                    // function GetAziende(divToBlock, success, error
                    jsCurrentMethodCall = @"function " + actionMethod
                        + @"(divToBlock, success, error";

                    //parametri GET e POST
                    var parametriGet = new List<string>();
                    var parametriPost = new List<string>();
                    if (apiModel.UriParameters != null)
                        parametriGet.AddRange(apiModel.UriParameters.Select(p => p.Name));
                    if (apiModel.RequestBodyParameters != null)
                        parametriPost.AddRange(apiModel.RequestBodyParameters.Select(p => p.Name));

                    //aggiungo la virgola e lo spazio dopo error se ci sono parametri
                    //function GetAziende(divToBlock, success, error, 
                    if (parametriGet.Count > 0 || parametriPost.Count > 0)
                    {
                        jsCurrentMethodCall += ", ";

                        //function GetAziende(divToBlock, success, error, param1, param2) {
                        jsCurrentMethodCall += string.Join(", ", parametriGet);
                        jsCurrentMethodCall += string.Join(", ", parametriPost);
                    }
                    jsCurrentMethodCall += @") {" + Environment.NewLine;

                    //Get(divToBlock, success, error, false, 
                    jsCurrentMethodCall += "\t"; //tab
                    jsCurrentMethodCall += isGet ? "Get" : "Post";
                    jsCurrentMethodCall += @"(divToBlock, success, error, "
                        + doesReturnJsonString + ", ";

                    //url dell'api (coi parametri GET da inserire nell'url)
                    string apiUrl = "\"/"  // "/
                        //api/metodo?param=asd&param2=asd
                        + relativePath.Replace("{", "\" + ").Replace("}", " + \"")
                        + "\"";

                    //cancello l'ultimo pezzo  + "" diventa stringa vuota
                    apiUrl = apiUrl.Replace(" + \"\"", string.Empty);

                    //Get(divToBlock, success, error, false, "api/metodo?param=asd&param2=asd"
                    jsCurrentMethodCall += apiUrl;

                    //Get(divToBlock, success, error, false, "api/metodo?param=asd&param2=asd", { param1: param1, param2: param2 });
                    if (parametriPost.Count > 0)
                    {
                        jsCurrentMethodCall += ", { " + string.Join(", ", parametriPost.Select(p => p + ": " + p)) + " }";
                    }
                    jsCurrentMethodCall += @");" + Environment.NewLine;

                    //chiudo il metodo
                    jsCurrentMethodCall += @"};";

                    //lo aggiungo alle call
                    jsMethodsCall += jsCurrentMethodCall + Environment.NewLine;
                }
            }

            string nuovoTemplateApi = ApiTemplate;

            //replace dei metodi e delle chiamate ai metodi
            nuovoTemplateApi = nuovoTemplateApi.Replace("{METHODS_CALL}", jsMethodsCall);

            var methodsNameBuilder = new StringBuilder();
            foreach (var item in ApiControllers)
            {
                methodsNameBuilder.AppendLine(@"//" + item.Key.ControllerName);
                methodsNameBuilder.AppendLine(string.Join(", " + Environment.NewLine,
                    item.Select(p => p.ActionDescriptor.ActionName + ": " + p.ActionDescriptor.ActionName))
                    + ",");
                methodsNameBuilder.AppendLine();
            }
            string methodsName = methodsNameBuilder.ToString();
            //rimuovo l'ultima virgola
            methodsName = methodsName.Remove(methodsName.LastIndexOf(','), 1);

            nuovoTemplateApi = nuovoTemplateApi.Replace("{METHODS_NAME}", methodsName);

            File.WriteAllText(ApiGeneratedPath, nuovoTemplateApi);

            //check se ci sono nomi doppi
            var metodiDoppi = ApiControllers.SelectMany(p => p.Select(q => q.ActionDescriptor.ActionName))
                .GroupBy(p => p)
                .Select(p => new { Key = p.Key, Count = p.Count() })
                .Where(p => p.Count > 1)
                .ToList();
            if (metodiDoppi.Count > 0)
                return BadRequest("File API creato ma alcuni metodi sono doppi: "
                    + string.Join(", ", metodiDoppi));

            var msgOk = "file api_generated.js generato, " + @"<br/>";
            string apiFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "App", "api.js");
            if (File.Exists(apiFilePath))
            {
                msgOk += "VS command window: Tools.DiffFiles"
                    + " \"" + ApiGeneratedPath + "\""
                    + " \"" + apiFilePath + "\"";
            }
            return Ok(msgOk);
        }

        [HttpGet]
        [Route("GenerateCommon")]
        public IHttpActionResult GenerateCommon()
        {
            var modelNames = new List<string>();
            string jsModels = string.Empty;
            foreach (var apiController in ApiControllers)
            {
                foreach (var apiMethod in apiController)
                {
                    //mi servono solo i metodi che ricevo in get
                    if (apiMethod.HttpMethod.Method != "GET")
                        continue;

                    string friendlyID = apiMethod.GetFriendlyId();
                    var apiModel = GlobalConfiguration.Configuration.GetHelpPageApiModel(friendlyID);

                    bool doesReturnJson = apiModel.ResourceDescription.ModelType.Name != "IHttpActionResult" ? true : false;
                    if (!doesReturnJson)
                        continue;

                    var modelsjs = GetCommonJsModels(apiModel.ResourceDescription);
                    foreach (var item in modelsjs)
                    {
                        if (modelNames.Contains(item.ModelName))
                            continue;
                        modelNames.Add(item.ModelName);
                        jsModels += item.ModelCommonJs;
                    }
                }
            }

            string nuovoTemplateCommon = CommonTemplate;

            //replace dei metodi e delle chiamate ai metodi
            nuovoTemplateCommon = nuovoTemplateCommon
                .Replace("{METHODS_CALL}", jsModels);

            nuovoTemplateCommon = nuovoTemplateCommon
                .Replace("{METHODS_NAME}",
                string.Join(", " + Environment.NewLine + "\t\t\t",
                modelNames.OrderBy(p => p).Select(p => p + ": " + p)));

            File.WriteAllText(CommonGeneratedPath, nuovoTemplateCommon);

            var msgOk = "file common_generated.js generato, " + @"<br/>";
            string commonFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "common.js");
            if (File.Exists(commonFilePath))
            {
                msgOk += "VS command window: Tools.DiffFiles"
                    + " \"" + CommonGeneratedPath + "\""
                    + " \"" + commonFilePath + "\"";
            }

            return Ok(msgOk);
        }

        /// <summary>
        /// generate ko-grids for all GET methods that returns collections
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GenerateGrids")]
        public IHttpActionResult GenerateGrids()
        {
            string templateClientGridT1_js = KoClientGridTemplateJs;
            string templateClientGridT1_html = KoClientGridTemplateHtml;

            //cartella per metterci gli altri file generati
            Directory.CreateDirectory(GeneratedPath);

            foreach (var apiController in ApiControllers)
            {
                foreach (var apiMethod in apiController)
                {
                    if (apiMethod.HttpMethod.Method != "GET")
                        continue;

                    string friendlyID = apiMethod.GetFriendlyId();
                    var apiModel = GlobalConfiguration.Configuration.GetHelpPageApiModel(friendlyID);

                    bool doesReturnJson = apiModel.ResourceDescription
                        .ModelType.Name != "IHttpActionResult" ? true : false;

                    if (!doesReturnJson)
                        continue;

                    string clientGridT1_js = templateClientGridT1_js;
                    string clientGridT1_html = templateClientGridT1_html;

                    string nomeModello = apiModel.ResourceDescription.Name;
                    var modelCollection = apiModel.ResourceDescription as CollectionModelDescription;
                    if (modelCollection == null)
                        continue;

                    var complexModel = modelCollection.ElementDescription as ComplexTypeModelDescription;
                    if (complexModel == null)
                        continue; //non genero knockout grids a partire da liste di oggetti base

                    nomeModello = modelCollection.ElementDescription.Name.Replace("Model", string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(nomeModello))
                        throw new Exception("Un Nome modello è vuoto o si chiama Model");

                    string nomeObsArray = ToJsName(nomeModello) //rendo minuscola la prima lettera
                                          + "s"; //pluralizzo aggiungendo una s alla fine

                    string nomeComponent = nomeObsArray.ToLowerInvariant(); // + "-grid";

                    //replace dai template
                    clientGridT1_js = clientGridT1_js.Replace("nomeModello", nomeModello);
                    clientGridT1_js = clientGridT1_js.Replace("nomeObsArray", nomeObsArray);
                    clientGridT1_js = clientGridT1_js.Replace("ActionName", apiMethod.ActionDescriptor.ActionName);
                    clientGridT1_js = clientGridT1_js.Replace("ko-clientgrid-t1", nomeComponent); //per il define all'inizio per trovare il file html relativo

                    var paramsConNavi = GetGridColumnsSorting(complexModel);

                    var headerSortingParams = paramsConNavi.Select(p =>
                        "<th><a href='#' data-bind=\"click: function(){ changeSort('" + p + "') }\">" + p + "</a></th>")
                        .ToList();

                    //add isSelected header for grids
                    headerSortingParams.AddRange(new string[] { "<th>",
                        "<input type=\"checkbox\" data-bind=\"checked: isAllSelected\" />",
                        "</th>" });

                    var headerDataParams = paramsConNavi
                        .Select(p => "<td data-bind=\"text: " + p + "\"></td>")
                        .ToList();

                    //add isSelected for grids
                    headerDataParams.AddRange(new string[] { "<td>",
                        "<input type=\"checkbox\" data-bind=\"checked: isSelected\" />",
                        "</td>" });

                    string headerSortingHtml = string.Join(Environment.NewLine, headerSortingParams);
                    string headerDataHtml = string.Join(Environment.NewLine, headerDataParams);

                    clientGridT1_html = clientGridT1_html.Replace("headerSortingHtml", headerSortingHtml);
                    clientGridT1_html = clientGridT1_html.Replace("headerDataHtml", headerDataHtml);
                    clientGridT1_html = clientGridT1_html.Replace("nomeObsArray", nomeObsArray);

                    //salvo i file
                    string containerFolder = Path.Combine(KoGridsContainerGeneratedPath, nomeComponent);
                    Directory.CreateDirectory(containerFolder);

                    string jsPath = Path.Combine(containerFolder, nomeComponent + ".js");
                    string htmlPath = Path.Combine(containerFolder, nomeComponent + ".html");
                    File.WriteAllText(jsPath, clientGridT1_js);
                    File.WriteAllText(htmlPath, clientGridT1_html);
                }
            }

            return Ok("Knockout Grids generated");
        }
#endif

        #region CommonJS
        public class CommonJsModel
        {
            public string ModelName { get; set; }
            public string ModelCommonJs { get; set; }
        }

        [NonAction]
        private List<CommonJsModel> GetCommonJsModels(ModelDescription ApiModel)
        {
            //in case of a collection I take the inner element instead
            var collection = ApiModel as CollectionModelDescription;
            if (collection != null)
                ApiModel = collection.ElementDescription;

            var complexModel = ApiModel as ComplexTypeModelDescription;
            if (complexModel == null)
                return new List<CommonJsModel>();

            var modelJsList = new List<CommonJsModel>();
            string modelJs = GetCommonJsModel(complexModel,
                ApiModel.Name.Replace("Model", string.Empty));
            modelJsList.Add(new CommonJsModel()
            {
                ModelName = ApiModel.Name.Replace("Model", string.Empty),
                ModelCommonJs = modelJs
            });
            foreach (var prop in complexModel.Properties)
            {
                modelJsList.AddRange(GetCommonJsModels(prop.TypeDescription));
            }
            return modelJsList;
        }

        [NonAction]
        private string GetCommonJsModel(ComplexTypeModelDescription ComplexModel, string ModelName)
        {
            string jsModels = string.Empty;

            // function TipologiaPrestazione(TipologiaPrestazione) {
            jsModels += "function " + ModelName + "(" + ModelName + ") {";

            jsModels += Environment.NewLine;
            jsModels += "\t";
            jsModels += "var self = this;";
            jsModels += Environment.NewLine + Environment.NewLine;

            foreach (var prop in ComplexModel.Properties)
            {
                jsModels += "\t";

                //self.id = TipologiaPrestazione.ID;
                if (typeof(DateTime) == prop.TypeDescription.ModelType
                   || typeof(DateTime?) == prop.TypeDescription.ModelType)
                    jsModels += "self." + ToJsName(prop.Name)
                        + " = new Data(" + ModelName + "." + prop.Name + ");";
                else
                    jsModels += "self." + ToJsName(prop.Name)
                        + " = " + ModelName + "." + prop.Name + ";";

                jsModels += Environment.NewLine;
            }

            jsModels += "};";
            jsModels += Environment.NewLine;
            jsModels += Environment.NewLine;

            return jsModels;
        }
        #endregion

        #region Utils
        public IEnumerable<IGrouping<HttpControllerDescriptor, ApiDescription>> ApiControllers
        {
            get
            {
                //Group APIs by controller
                var model = GlobalConfiguration.Configuration.Services
                    .GetApiExplorer().ApiDescriptions;
                ILookup<HttpControllerDescriptor, ApiDescription> apiGroups = model
                    .ToLookup(api => api.ActionDescriptor.ControllerDescriptor);
                return apiGroups.OrderBy(p => p.Key.ControllerName)
                    //Exclude generation for this controller
                    .Where(p => p.Key.ControllerName != "JavascriptGenerator")
                    .ToList();
            }
        }
        
        [NonAction]
        private List<string> GetGridColumnsSorting(ComplexTypeModelDescription ComplexModel)
        {
            var properties = new List<string>();
            foreach (var par in ComplexModel.Properties)
            {
                var simplePar = par.TypeDescription as SimpleTypeModelDescription;
                if (simplePar != null)
                {
                    properties.Add(ToJsName(par.Name));
                    continue;
                }

                //chiamata ricorsiva per complex model
                var innerComplexModel = par.TypeDescription as ComplexTypeModelDescription;
                if (innerComplexModel != null)
                {
                    var innerProperties = GetGridColumnsSorting(innerComplexModel);

                    //aggiungo navigazione (ad. es reparto.nome)
                    var innerPropertiesWithNavi = innerProperties.Select(p => ToJsName(par.Name) + "." + p);
                    properties.AddRange(innerPropertiesWithNavi);
                    continue;
                }
            }
            return properties;
        }
        
        [NonAction]
        private string ToJsName(string Testo)
        {
            //eccezioni
            if (Testo == "CAP")
                return Testo;
            if (Testo == "ID")
                return "id";

            if (Testo.StartsWith("ID"))
                return "id" + Testo.Substring(2);

            //altrimenti metto a minuscola solo la prima lettera
            return Testo.Substring(0, 1).ToLowerInvariant()
                                        + Testo.Substring(1, Testo.Length - 1);
        }
        #endregion

        #region Paths
        public static string AppGeneratePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App-generate");
            }
        }
        public static string TemplatesPath
        {
            get
            {
                return Path.Combine(AppGeneratePath, "templates");
            }
        }
        public static string GeneratedPath
        {
            get
            {
                string path = Path.Combine(AppGeneratePath, "generated");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }
        public static string ApiTemplate
        {
            get
            {
                string filePath = Path.Combine(TemplatesPath, "api.js");
                return File.ReadAllText(filePath);
            }
        }
        public static string CommonTemplate
        {
            get
            {
                string filePath = Path.Combine(TemplatesPath, "common.js");
                return File.ReadAllText(filePath);
            }
        }
        public static string KoClientGridTemplateHtml
        {
            get
            {
                string filePath = Path.Combine(TemplatesPath, "ko-clientgrid-t1", "ko-clientgrid-t1.html");
                return File.ReadAllText(filePath);
            }
        }
        public static string KoClientGridTemplateJs
        {
            get
            {
                string filePath = Path.Combine(TemplatesPath, "ko-clientgrid-t1", "ko-clientgrid-t1.js");
                return File.ReadAllText(filePath);
            }
        }
        //Where to place the generated files
        public static string ApiGeneratedPath
        {
            get
            {
                string filePath = Path.Combine(GeneratedPath, "api.js");
                return filePath;
            }
        }
        public static string CommonGeneratedPath
        {
            get
            {
                string filePath = Path.Combine(GeneratedPath, "common.js");
                return filePath;
            }
        }
        public static string KoGridsContainerGeneratedPath
        {
            get
            {
                string containerFolder = Path.Combine(GeneratedPath, "KoGrids");
                return containerFolder;
            }
        }
        #endregion
    }
}