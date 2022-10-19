using B21leowa_DOTNet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static Google.Protobuf.Reflection.UninterpretedOption.Types;

namespace B21leowa_DOTNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;
        private BarnModel _barnModel;
        private ChildRelationModel _childRelationModel;
        private WishListModel _wishListModel;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _barnModel = new BarnModel(_configuration);
            _childRelationModel = new ChildRelationModel(_configuration);
            _wishListModel = new WishListModel(_configuration);
        }

        //VIEWS
        public IActionResult Index()
        {
            ViewBag.barnTable = _barnModel.GetAllChildren();
            ViewBag.childRelationTable = _childRelationModel.GetAllChildRelation();
            ViewBag.wishListTable = _wishListModel.GetAllWishes();
            return View();
        }

        public IActionResult CreateChildView()
        {
            ViewBag.barnTable = _barnModel.GetAllChildren();
            return View();
        }

        public IActionResult CreateChildRelationView()
        {
            ViewBag.barnTable = _barnModel.GetAllChildren();
            return View();
        }

        public IActionResult CreateWishListView()
        {
            ViewBag.barnTable = _barnModel.GetAllChildren();
            return View();
        }

        public IActionResult SearchChildView(string name)
        {
            ViewBag.SearchResult = _barnModel.SearchChildren(name);
            return View();
        }

        //ACTIONS
        public IActionResult InsertChild(string PNR,  string firstname, string surname, string birthday, int kindnessScale, string pwd)
        {
            
            _barnModel.InsertChild(PNR, firstname, surname, birthday, kindnessScale, pwd);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteChild(string PNR, string name)
        {
            _barnModel.DeleteChild(PNR, name);
            return RedirectToAction("Index");
        }

        public IActionResult InsertChildRelation(string namePNR1, string namePNR2, string typeOfRelation)
        {
            _childRelationModel.InsertChildRelation(namePNR1, namePNR2, typeOfRelation);
            return RedirectToAction("Index");
        }
        
        public IActionResult DeleteChildRelation(string PNR1, string name1, string PNR2, string name2)
        {
            _childRelationModel.DeleteChildRelation(PNR1, name1, PNR2, name2);
            return RedirectToAction("Index");
        }

        public IActionResult InsertWish(string namePNR, string description)
        {
            _wishListModel.CreateWish(namePNR, description);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteWish(string createdDate, string PNR)
        {
            _wishListModel.DeleteWish(createdDate, PNR);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}