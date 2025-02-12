﻿using CloudAwesome.Xrm.Customisation.Models;

namespace CloudAwesome.Xrm.Customisation
{
    public interface ICustomisationManifest
    {
        LoggingConfiguration LoggingConfiguration { get; set; }
        CdsConnection CdsConnection { get; set; }
    }
}