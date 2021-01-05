# CloudAwesome.Xrm.Customisation

> Automate common, time-consuming and error prone tasks during customisation and configuration of Dynamics 365/CDS

![In progress](documentation/assets/Status-InProgress.svg)

## Features

- [Register plugins from XML manifest](documentation/features/plugin-registration/plugin-registration.md)
    - Plugins, steps, service endpoints, workflow assemblies, webhooks
    - Keep plugin registration details in source alongside the plugin assemblies so don't need to open the Plugin Reg Tool and configure manually
- [Generate customisations from XML manifest](documentation/features/generate-customisations/generate-customisations.md)
    - Entities, forms, views, optionsets
    - Security roles and field level security profiles
    - Model driven apps and sitemap
    - Quickly generate, test and tear down all artifacts during prototyping phases
- [Migrate Bulk Deletion Jobs between environments](documentation/features/bulk-deletion-jobs/bulk-deletion-jobs.md)
- [Toggle process status from XML manifest](documentation/features/toggle-process-status/toggle-process-status.md)
    - Activate/Deactivate processes specified individually, included in solutions, or parented by assembly
    - Useful for data migrations/imports
    - Include in source control which processes should be disabled (or which shouldn't be re-enabled)
    - Supports plugin steps, workflows, modern flows and case creation rules
