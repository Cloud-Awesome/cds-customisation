# CloudAwesome.Xrm.Customisation
Solutions to automate common and time-consuming tasks during customisation of Dynamics 365/CDS

![In progress](documentation/assets/Status-InProgress.svg)

## Features
- Generate customisations from XML manifest
    - Entities, forms, views, optionsets
    - Security roles and field level security profiles
    - Model driven apps and sitemap
    - Quickly generate, test and tear down all artifacts during prototyping phases
- Register plugins from XML manifest
    - Plugins, steps, service endpoints, workflow assemblies, webhooks
    - Keep plugin registration details in source alongside the plugin assemblies so don't need to open the Plugin Reg Tool and configure manually
- Migrate Bulk Deletion Jobs between environments
- Toggle process status from XML manifest
    - Activate/Deactivate processes specified individually, included in solutions, or parented by assembly
    - Useful for data migrations/imports
    - Include in source control which processes should be disabled (or which shouldn't be re-enabled)

