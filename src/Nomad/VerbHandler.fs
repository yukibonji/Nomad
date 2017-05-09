namespace Nomad.Verbs

open Nomad

/// A handler for each Http Verb
type VerbHandler<'T> =
    {
        /// A handler for GET requests
        Get : HttpHandler<'T>
        /// A handler for POST requests
        Post : HttpHandler<'T>
        /// A handler for PUT requests
        Put : HttpHandler<'T>
        /// A handler for PATCH requests
        Patch : HttpHandler<'T>
        /// A handler for DELETE requests
        Delete : HttpHandler<'T>
    }

module HttpHandler =
    /// A set of default http verb handlers, they return Error 405 : Method Not Allowed in response to all requests.  Use `defaultVerbs with` syntax to specify specific verb handlers.
    let defaultVerbs =
        {
            Get    = Responses.``Method Not Allowed``
            Post   = Responses.``Method Not Allowed``
            Put    = Responses.``Method Not Allowed``
            Patch  = Responses.``Method Not Allowed``
            Delete = Responses.``Method Not Allowed``
        }

    /// Handle a set of http verb handlers
    let handleVerbs (verbHandler : VerbHandler<'T>) = 
        HttpHandler (fun ctx -> 
            match Http.requestMethod ctx.Request.Method with
            |Get -> InternalHandlers.runHandler verbHandler.Get ctx
            |Post -> InternalHandlers.runHandler verbHandler.Post ctx
            |Put -> InternalHandlers.runHandler verbHandler.Put ctx
            |Patch -> InternalHandlers.runHandler verbHandler.Patch ctx
            |Delete -> InternalHandlers.runHandler verbHandler.Delete ctx
        )