namespace Suteki.TardisBank.Web.Mvc.Controllers
{
    using System.Web.Mvc;

    using Raven.Client;
    using Raven.Client.Linq;

    using SharpArch.RavenDb.Web.Mvc;

    using Suteki.TardisBank.Domain;
    using Suteki.TardisBank.Tasks;
    using Suteki.TardisBank.Web.Mvc.Controllers.ViewModels;
    using Suteki.TardisBank.Web.Mvc.Utilities;

    public class ChildController : Controller
    {
        readonly IUserService userService;

        private readonly IDocumentSession session;

        public ChildController(IUserService userService, IDocumentSession session)
        {
            this.userService = userService;
            this.session = session;
        }

        [HttpGet, UnitOfWork]
        public ActionResult Index()
        {
            var parent = this.userService.CurrentUser as Parent;
            if (parent == null)
            {
                return StatusCode.NotFound;
            }

            var children = this.session.Query<Child>().Where(c => c.ParentId == parent.Id);
            return this.View(children);
        }

        [HttpGet, UnitOfWork]
        public ActionResult DeleteChild(string id)
        {
            // id is the child's user name
            var child = this.userService.GetUser(id) as Child;
            if (child == null)
            {
                return StatusCode.NotFound;
            }

            return this.View(new DeleteChildConfirmViewModel
            {
                ChildId = child.Id,
                ChildName = child.Name
            });
        }

        [HttpPost, UnitOfWork]
        public ActionResult DeleteChild(DeleteChildConfirmViewModel deleteChildConfirmViewModel)
        {
            var parent = this.userService.CurrentUser as Parent;
            if (parent == null || !parent.HasChild(deleteChildConfirmViewModel.ChildId))
            {
                return StatusCode.NotFound;
            }
            parent.RemoveChild(deleteChildConfirmViewModel.ChildId);
            this.userService.DeleteUser(deleteChildConfirmViewModel.ChildId);
            return this.RedirectToAction("Index");
        }
    }
}