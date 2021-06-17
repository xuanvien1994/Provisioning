using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Covergo.AADProvisioning.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Microsoft.SystemForCrossDomainIdentityManagement;
using AttributeNames = Covergo.AADProvisioning.Schema.AttributeNames;
using ProtocolConstants = Covergo.AADProvisioning.Protocal.ProtocolConstants;
using QueryResponseBase = Covergo.AADProvisioning.Protocal.QueryResponseBase;
using RequestExtensions = Covergo.AADProvisioning.Service.RequestExtensions;
using Resource = Covergo.AADProvisioning.Schema.Resource;
using SchemaIdentifiers = Covergo.AADProvisioning.Schema.SchemaIdentifiers;

//using Microsoft.SystemForCrossDomainIdentityManagement;

namespace Covergo.AADProvisioning.Controllers
{
    public abstract class ControllerTemplate : ControllerBase
    {
        internal const string AttributeValueIdentifier = "{identifier}";
        private const string HeaderKeyContentType = "Content-Type";
        private const string HeaderKeyLocation = "Location";

        internal readonly IMonitor Monitor;
        internal readonly Service.IProvider Provider;
        internal ControllerTemplate(Service.IProvider provider, IMonitor monitor)
        {
            Monitor = monitor;
            Provider = provider;
        }

        protected virtual void ConfigureResponse(Schema.Resource resource)
        {
            Response.ContentType = ProtocolConstants.ContentType;
            Response.StatusCode = (int)HttpStatusCode.Created;

            if (null == Response.Headers)
            {
                return;
            }

            if (!Response.Headers.ContainsKey(HeaderKeyContentType))
            {
                Response.Headers.Add(HeaderKeyContentType, ProtocolConstants.ContentType);
            }

            Uri baseResourceIdentifier = RequestExtensions.GetBaseResourceIdentifier(this.ConvertRequest());
            var resourceIdentifier = Protocal.ProtocolExtensions.GetResourceIdentifier(resource, baseResourceIdentifier);
            string resourceLocation = resourceIdentifier.AbsoluteUri;
            if (!this.Response.Headers.ContainsKey(HeaderKeyLocation))
            {
                this.Response.Headers.Add(HeaderKeyLocation, resourceLocation);
            }
        }

        protected HttpRequestMessage ConvertRequest()
        {
            HttpRequestMessageFeature hreqmf = new HttpRequestMessageFeature(this.HttpContext);
            HttpRequestMessage result = hreqmf.HttpRequestMessage;
            return result;
        }

        protected virtual bool TryGetMonitor(out IMonitor monitor)
        {
            monitor = Monitor;

            return null != monitor;
        }
    }

    public abstract class ControllerTemplate<T> : ControllerTemplate where T : Resource
    {
        internal ControllerTemplate(Service.IProvider provider, IMonitor monitor)
            : base(provider, monitor)
        {
        }

        protected abstract Service.IProviderAdapter<T> AdaptProvider(Service.IProvider provider);

        protected virtual Service.IProviderAdapter<T> AdaptProvider()
        {
            Service.IProviderAdapter<T> result = this.AdaptProvider(this.Provider);
            return result;
        }


        [HttpDelete(ControllerTemplate.AttributeValueIdentifier)]
        public virtual async Task<IActionResult> Delete(string identifier)
        {
            string correlationIdentifier = null;
            try
            {
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    return this.BadRequest();
                }

                identifier = Uri.UnescapeDataString(identifier);
                HttpRequestMessage request = this.ConvertRequest();
                if (!RequestExtensions.TryGetRequestIdentifier(request, out correlationIdentifier))
                {
                    throw new NotImplementedException();
                }

                Service.IProviderAdapter<T> provider = this.AdaptProvider();
                await provider.Delete(request, identifier, correlationIdentifier).ConfigureAwait(false);
                return this.NoContent();
            }
            catch (ArgumentException argumentException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            argumentException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplateDeleteArgumentException);
                    monitor.Report(notification);
                }

                return this.BadRequest();
            }
            //catch (HttpResponseException responseException)
            //{
            //    if (responseException.Response?.StatusCode == HttpStatusCode.NotFound)
            //    {
            //        return this.NotFound();
            //    }

            //    throw;
            //}
            catch (NotImplementedException notImplementedException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            notImplementedException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplateDeleteNotImplementedException);
                    monitor.Report(notification);
                }

                throw;
            }
            catch (NotSupportedException notSupportedException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            notSupportedException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplateDeleteNotSupportedException);
                    monitor.Report(notification);
                }

                throw;
            }
            catch (Exception exception)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            exception,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplateDeleteException);
                    monitor.Report(notification);
                }

                throw;
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "The names of the methods of a controller must correspond to the names of hypertext markup verbs")]
        public virtual async Task<ActionResult<QueryResponseBase>> Get()
        {
            string correlationIdentifier = null;
            try
            {
                HttpRequestMessage request = this.ConvertRequest();
                if (!RequestExtensions.TryGetRequestIdentifier(request, out correlationIdentifier))
                {
                    throw new NotImplementedException();
                }

                IResourceQuery resourceQuery = new ResourceQuery(request.RequestUri);
                Service.IProviderAdapter<T> provider = this.AdaptProvider();
                QueryResponseBase result =
                    await provider
                            .Query(
                                request,
                                resourceQuery.Filters,
                                resourceQuery.Attributes,
                                resourceQuery.ExcludedAttributes,
                                resourceQuery.PaginationParameters,
                                correlationIdentifier)
                            .ConfigureAwait(false);
                return this.Ok(result);
            }
            catch (ArgumentException argumentException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            argumentException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplateQueryArgumentException);
                    monitor.Report(notification);
                }

                return this.BadRequest();
            }
            catch (NotImplementedException notImplementedException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            notImplementedException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplateQueryNotImplementedException);
                    monitor.Report(notification);
                }

                throw;
            }
            catch (NotSupportedException notSupportedException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            notSupportedException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplateQueryNotSupportedException);
                    monitor.Report(notification);
                }

                throw;
            }
            //catch (HttpResponseException responseException)
            //{
            //    if (responseException.Response?.StatusCode != HttpStatusCode.NotFound)
            //    {
            //        if (this.TryGetMonitor(out IMonitor monitor))
            //        {
            //            IExceptionNotification notification =
            //                ExceptionNotificationFactory.Instance.CreateNotification(
            //                    responseException.InnerException ?? responseException,
            //                    correlationIdentifier,
            //                    ServiceNotificationIdentifiers.ControllerTemplateGetException);
            //            monitor.Report(notification);
            //        }
            //    }

            //    throw;
            //}
            catch (Exception exception)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            exception,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplateQueryException);
                    monitor.Report(notification);
                }

                throw;
            }
        }

        [Microsoft.AspNetCore.Mvc.HttpGet(ControllerTemplate.AttributeValueIdentifier)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "The names of the methods of a controller must correspond to the names of hypertext markup verbs")]
        public virtual async Task<IActionResult> Get(string identifier)
        {
            string correlationIdentifier = null;
            try
            {
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    return this.BadRequest();
                }

                HttpRequestMessage request = this.ConvertRequest();
                if (!RequestExtensions.TryGetRequestIdentifier(request, out correlationIdentifier))
                {
                    throw new NotImplementedException();
                }

                IResourceQuery resourceQuery = new ResourceQuery(request.RequestUri);
                if (resourceQuery.Filters.Any())
                {
                    if (resourceQuery.Filters.Count != 1)
                    {
                        return this.BadRequest();
                    }

                    IFilter filter = new Filter(AttributeNames.Identifier, ComparisonOperator.Equals, identifier);
                    filter.AdditionalFilter = resourceQuery.Filters.Single();
                    IReadOnlyCollection<IFilter> filters =
                        new IFilter[]
                            {
                                filter
                            };
                    IResourceQuery effectiveQuery =
                        new ResourceQuery(
                            filters,
                            resourceQuery.Attributes,
                            resourceQuery.ExcludedAttributes);
                    Service.IProviderAdapter<T> provider = this.AdaptProvider();
                    Protocal.QueryResponseBase queryResponse =
                        await provider
                            .Query(
                                request,
                                effectiveQuery.Filters,
                                effectiveQuery.Attributes,
                                effectiveQuery.ExcludedAttributes,
                                effectiveQuery.PaginationParameters,
                                correlationIdentifier)
                            .ConfigureAwait(false);
                    if (!queryResponse.Resources.Any())
                    {
                        return this.NotFound();
                    }

                    Resource result = queryResponse.Resources.Single();
                    return this.Ok(result);
                }
                else
                {
                    Service.IProviderAdapter<T> provider = this.AdaptProvider();
                    Resource result =
                        await provider
                            .Retrieve(
                                request,
                                identifier,
                                resourceQuery.Attributes,
                                resourceQuery.ExcludedAttributes,
                                correlationIdentifier)
                            .ConfigureAwait(false);
                    if (null == result)
                    {
                        return this.NotFound();
                    }

                    return this.Ok(result);
                }
            }
            catch (ArgumentException argumentException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            argumentException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplateGetArgumentException);
                    monitor.Report(notification);
                }

                return this.BadRequest();
            }
            catch (NotImplementedException notImplementedException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            notImplementedException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplateGetNotImplementedException);
                    monitor.Report(notification);
                }

                throw;
            }
            catch (NotSupportedException notSupportedException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            notSupportedException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplateGetNotSupportedException);
                    monitor.Report(notification);
                }

                throw;
            }
            //catch (HttpResponseException responseException)
            //{
            //    if (responseException.Response?.StatusCode != HttpStatusCode.NotFound)
            //    {
            //        if (this.TryGetMonitor(out IMonitor monitor))
            //        {
            //            IExceptionNotification notification =
            //                ExceptionNotificationFactory.Instance.CreateNotification(
            //                    responseException.InnerException ?? responseException,
            //                    correlationIdentifier,
            //                    ServiceNotificationIdentifiers.ControllerTemplateGetException);
            //            monitor.Report(notification);
            //        }
            //    }

            //    if (responseException.Response?.StatusCode == HttpStatusCode.NotFound)
            //    {
            //        return this.NotFound();
            //    }

            //    throw;
            //}
            catch (Exception exception)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            exception,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplateGetException);
                    monitor.Report(notification);
                }

                throw;
            }
        }

        [HttpPatch(ControllerTemplate.AttributeValueIdentifier)]
        public virtual async Task<IActionResult> Patch(string identifier, [FromBody] PatchRequest2 patchRequest)
        {
            string correlationIdentifier = null;

            try
            {
                if (string.IsNullOrWhiteSpace(identifier))
                {
                    return this.BadRequest();
                }

                identifier = Uri.UnescapeDataString(identifier);

                if (null == patchRequest)
                {
                    return this.BadRequest();
                }

                HttpRequestMessage request = this.ConvertRequest();
                if (!RequestExtensions.TryGetRequestIdentifier(request, out correlationIdentifier))
                {
                    throw new NotImplementedException();
                }

                Service.IProviderAdapter<T> provider = this.AdaptProvider();
                await provider.Update(request, identifier, patchRequest, correlationIdentifier).ConfigureAwait(false);

                // If EnterpriseUser, return HTTP code 200 and user object, otherwise HTTP code 204
                if (provider.SchemaIdentifier == SchemaIdentifiers.Core2EnterpriseUser)
                {
                    return await this.Get(identifier).ConfigureAwait(false);
                }
                else
                    return this.NoContent();
            }
            catch (ArgumentException argumentException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            argumentException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplatePatchArgumentException);
                    monitor.Report(notification);
                }

                return this.BadRequest();
            }
            catch (NotImplementedException notImplementedException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            notImplementedException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplatePatchNotImplementedException);
                    monitor.Report(notification);
                }

                throw;
            }
            catch (NotSupportedException notSupportedException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            notSupportedException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplatePatchNotSupportedException);
                    monitor.Report(notification);
                }

                throw;
            }
            //catch (HttpResponseException responseException)
            //{
            //    if (responseException.Response?.StatusCode == HttpStatusCode.NotFound)
            //    {
            //        return this.NotFound();
            //    }
            //    else
            //    {
            //        //if (this.TryGetMonitor(out IMonitor monitor))
            //        //{
            //        //    IExceptionNotification notification =
            //        //        ExceptionNotificationFactory.Instance.CreateNotification(
            //        //            responseException.InnerException ?? responseException,
            //        //            correlationIdentifier,
            //        //            ServiceNotificationIdentifiers.ControllerTemplateGetException);
            //        //    monitor.Report(notification);
            //        //}
            //    }

            //    throw;

            //}
            catch (Exception exception)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            exception,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplatePatchException);
                    monitor.Report(notification);
                }

                throw;
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult<Resource>> Post([FromBody] T resource)
        {
            string correlationIdentifier = null;

            try
            {
                if (null == resource)
                {
                    return this.BadRequest();
                }

                HttpRequestMessage request = this.ConvertRequest();
                if (!RequestExtensions.TryGetRequestIdentifier(request, out correlationIdentifier))
                {
                    throw new NotImplementedException();
                }

                Service.IProviderAdapter<T> provider = this.AdaptProvider();
                Resource result = await provider.Create(request, resource, correlationIdentifier).ConfigureAwait(false);
                this.ConfigureResponse(result);
                return this.CreatedAtAction(nameof(Post), result);
            }
            catch (ArgumentException argumentException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            argumentException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplatePostArgumentException);
                    monitor.Report(notification);
                }

                return this.BadRequest();
            }
            catch (NotImplementedException notImplementedException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            notImplementedException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplatePostNotImplementedException);
                    monitor.Report(notification);
                }

                throw;
            }
            //catch (NotSupportedException notSupportedException)
            //{
            //    if (this.TryGetMonitor(out IMonitor monitor))
            //    {
            //        IExceptionNotification notification =
            //            ExceptionNotificationFactory.Instance.CreateNotification(
            //                notSupportedException,
            //                correlationIdentifier,
            //                ServiceNotificationIdentifiers.ControllerTemplatePostNotSupportedException);
            //        monitor.Report(notification);
            //    }

            //    throw new HttpResponseException(HttpStatusCode.NotImplemented);
            //}
            //catch (HttpResponseException httpResponseException)
            //{
            //    if (this.TryGetMonitor(out IMonitor monitor))
            //    {
            //        IExceptionNotification notification =
            //            ExceptionNotificationFactory.Instance.CreateNotification(
            //                httpResponseException,
            //                correlationIdentifier,
            //                ServiceNotificationIdentifiers.ControllerTemplatePostNotSupportedException);
            //        monitor.Report(notification);
            //    }

            //    if (httpResponseException.Response.StatusCode == HttpStatusCode.Conflict)
            //        return this.Conflict();
            //    else
            //        return this.BadRequest();
            //}
            catch (Exception exception)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            exception,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplatePostException);
                    monitor.Report(notification);
                }

                throw;
            }
        }

        [HttpPut(AttributeValueIdentifier)]
        public virtual async Task<ActionResult<Resource>> Put([FromBody] T resource, string identifier)
        {
            string correlationIdentifier = null;

            try
            {
                if (null == resource)
                {
                    return this.BadRequest();
                }

                if (string.IsNullOrEmpty(identifier))
                {
                    return this.BadRequest();
                }

                HttpRequestMessage request = this.ConvertRequest();
                if (!RequestExtensions.TryGetRequestIdentifier(request, out correlationIdentifier))
                {
                }

                Service.IProviderAdapter<T> provider = this.AdaptProvider();
                Resource result = await provider.Replace(request, resource, correlationIdentifier).ConfigureAwait(false);
                this.ConfigureResponse(result);
                return this.Ok(result);
            }
            catch (ArgumentException argumentException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            argumentException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplatePutArgumentException);
                    monitor.Report(notification);
                }

                return this.BadRequest();
            }
            catch (NotImplementedException notImplementedException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            notImplementedException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplatePutNotImplementedException);
                    monitor.Report(notification);
                }

                throw;
            }
            catch (NotSupportedException notSupportedException)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            notSupportedException,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplatePutNotSupportedException);
                    monitor.Report(notification);
                }

                throw;
            }
            catch (Exception exception)
            {
                if (this.TryGetMonitor(out IMonitor monitor))
                {
                    IExceptionNotification notification =
                        ExceptionNotificationFactory.Instance.CreateNotification(
                            exception,
                            correlationIdentifier,
                            ServiceNotificationIdentifiers.ControllerTemplatePutException);
                    monitor.Report(notification);
                }

                throw;
            }
        }
    }
}
