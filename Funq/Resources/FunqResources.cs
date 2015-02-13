//
// This file genenerated by the Buckle tool on 2/12/2015 at 5:56 PM. 
//
// Contains strongly typed wrappers for resources in FunqResources.strings
//

namespace Funq {
using System;
using System.Reflection;
using System.Resources;
using System.Diagnostics;
using System.Globalization;


/// <summary>
/// Strongly typed resource wrappers generated from FunqResources.strings.
/// </summary>
public class FunqResources
{
    internal static readonly ResourceManager ResourceManager = new ResourceManager(typeof(FunqResources));

    /// <summary>
    /// Container service is built-in and read-only.
    /// </summary>
    public static string Registration_CantRegisterContainer
    {
        get
        {
            return ResourceManager.GetString("Registration_CantRegisterContainer", CultureInfo.CurrentUICulture);
        }
    }

    /// <summary>
    /// Service type {0} does not inherit or implement {1}.
    /// </summary>
    public static string Registration_IncompatibleAsType(object param0, object param1)
    {
        string format = ResourceManager.GetString("Registration_IncompatibleAsType", CultureInfo.CurrentUICulture);
        return string.Format(CultureInfo.CurrentCulture, format, param0, param1);
    }

    /// <summary>
    /// Required dependency of type {0} named '{1}' could not be resolved.
    /// </summary>
    public static string ResolutionException_MissingNamedType(object param0, object param1)
    {
        string format = ResourceManager.GetString("ResolutionException_MissingNamedType", CultureInfo.CurrentUICulture);
        return string.Format(CultureInfo.CurrentCulture, format, param0, param1);
    }

    /// <summary>
    /// Required dependency of type {0} could not be resolved.
    /// </summary>
    public static string ResolutionException_MissingType(object param0)
    {
        string format = ResourceManager.GetString("ResolutionException_MissingType", CultureInfo.CurrentUICulture);
        return string.Format(CultureInfo.CurrentCulture, format, param0);
    }

    /// <summary>
    /// Unknown scope.
    /// </summary>
    public static string ResolutionException_UnknownScope
    {
        get
        {
            return ResourceManager.GetString("ResolutionException_UnknownScope", CultureInfo.CurrentUICulture);
        }
    }

    /// <summary>
    /// Error trying to resolve Service '{0}' or one of its autowired dependencies (see inner exception for details).
    /// </summary>
    public static string ResolutionException_Autowired(object param0)
    {
        string format = ResourceManager.GetString("ResolutionException_Autowired", CultureInfo.CurrentUICulture);
        return string.Format(CultureInfo.CurrentCulture, format, param0);
    }

    /// <summary>
    /// Error trying to resolve Service '{0}' from Adapter '{1}': {2}
    /// </summary>
    public static string ResolutionException_Adapter(object param0, object param1, object param2)
    {
        string format = ResourceManager.GetString("ResolutionException_Adapter", CultureInfo.CurrentUICulture);
        return string.Format(CultureInfo.CurrentCulture, format, param0, param1, param2);
    }
}
}
