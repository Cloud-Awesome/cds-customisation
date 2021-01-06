# Manifest organisation patterns

> The `<SolutionName>` node (which CRM solution to add the assembly to) can exist both at the root level of a manifest and also as a child of each `<PluginAssembly>` node. If the `<SolutionName>` node exists under a `<PluginAssembly>` node, it overides the root value
>
> This allows you to manage registration of assemblies in a number of ways to match your CRM solution architecture, team(s) structure plus your ALM and deployment strategy

## 1. One manifest, one solution

- Everything is in a single solution
- Everything gets deployed at the same time
- Suitable for smaller teams

## 2. One manifest, multiple solutions

- All plugins get updated at the same time
- Solutions can be deployed to other environments as required
- Could cause deployment issues if CRM solutions have cross-dependencies (or if different teams/developers are adding/remving plugin types without keeping the manifest up to date)
- Suitable for smaller teams with regular or simultaneous deployments, or solutions with highly segregated dependencies

## 3. One manifest per business area

- Create a manifest to separate concerns, e.g. sale-manifest, support-manifest, field-service-manifest
- Allows teams at scale to develop and deploy their code in isolation
- Plurality of solutions is then optional and decided by each team
- Allows build and release pipelines to be triggered on a more granular basis

## 4. One manifest per architecture type

- Create a manifest for different technical areas, e.g. dynamics-application-manifest, integration-plugins-manifest, service-endpoint-manifest, data-migration-plugin-manifest
- Different technical areas often progress at different times and different velocities
- Plurality of solutions again is optional and controlled by the tehcnical teacm
- Allows build and release pipelines to be triggered on a more granular basis

For anything other than smaller (2-3 developer) team projects, I usually prefer either patterns 3, 4 or a combination of the two. Because the registration of multiple manifests can be scripted and/or triggered in a pipeline, multiple manifests are usually safer than fewer.
