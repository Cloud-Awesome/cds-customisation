# CloudAwesome.Xrm.Customisation

> Automate common, time-consuming, error prone, or otherwise non-automatable tasks during customisation and configuration of Dataverse/Dynamics 365 CE

![In progress](documentation/assets/Status-InProgress.svg)

## Features

- [Register plugins from manifest](documentation/features/plugin-registration/plugin-registration.md)
    - Automate registration of plugins, steps, service endpoints, workflow assemblies, webhooks
    - Keep all plugin registration details (steps, images, custom APIs, etc.) under source control alongside the plugin assembly code itself so that definition of steps etc is part of the development process. Don't need to open the Plugin Reg Tool and configure manually
- [Generate customisations from manifest](documentation/features/generate-customisations/generate-customisations.md)
    - Entities, forms, views, optionsets
    - Security roles and field level security profiles
    - Model driven apps and sitemap
    - Quickly generate, test and tear down all artifacts during prototyping phases
    - Enforce best practices often forgotten, such as ensuring schema names are all lower case
- [Toggle process status from manifest](documentation/features/toggle-process-status/toggle-process-status.md)
    - Activate/Deactivate processes specified individually, those included in specified solutions, or parented by assembly
    - Useful for data migrations/imports
    - Include in source control which processes should be disabled (or which shouldn't be re-enabled)
    - Supports plugin steps, workflows, modern flows and case creation rules
- [Migrate Bulk Deletion Jobs between environments](documentation/features/bulk-deletion-jobs/bulk-deletion-jobs.md)
- Manage security role assignment for teams and service accounts
    - Generate a manifest of team-role assignments from source environment (and commit that manifest to source control)
    - Import team-role assignments from manifest (auto- or manually-generated) to target environment


## Installation

Two versions are available on Nuget.

1. To use a Console Application for stand alone use or for use within a pipeline [CloudAwesome.Xrm.Customisation.Cli](https://www.nuget.org/packages/CloudAwesome.Xrm.Customisation.Cli/)
2. To integrate the API into your own solution, add the [CloudAwesome.Xrm.Customisation](https://www.nuget.org/packages/CloudAwesome.Xrm.Customisation/) package in your project

## Example usage

[[[ More documentation is en route! ;) ]]]
