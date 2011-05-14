using System.Web.Mvc;
using Castle.Windsor;
using System;
using System.Web.Routing;

namespace Ploeh.Samples.Booking.WebUI
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IWindsorContainer container;

        public WindsorControllerFactory(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return (IController)this.container.Resolve(controllerType);
        }

        public override void ReleaseController(IController controller)
        {
            this.container.Release(controller);
        }
    }
}