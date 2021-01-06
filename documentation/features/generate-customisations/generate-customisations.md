# Generate Customisations

> API and Command line application which processes an XML manifest to programatically create:
> - Entities, attributes, forms, views and global optionsets
> - Model driven apps and sitemaps
> - Security roles and field level profiles
> 
> Quickly generate, test and tear down data model and UI artifacts during prototyping phases, save time and prevent typos when generating large data models. Or integrate into an existing solution to migrate or synchronise data model from a 3rd party application into CDS

## Manifest

### XSD 

The correct version of manifest schema definition (xsd) is included in the source and *will* (to be: are) also available online here: https://cloudawesome.xyz/cloudawesome.xrm.customisation/schemas

The customisations manifest consists of the following top-level nodes:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ConfigurationManifest>
  <SolutionName>EntityCreationSandbox</SolutionName>
  <Clobber>false</Clobber>

  <!-- Entities, attributes, views, forms, and update security roles to define access -->
  <Entities></Entities>

  <!-- Global option sets -->
  <OptionSets></OptionSets>

  <!-- Create or update security roles. Configure non-entity related permissions -->
  <SecurityRoles></SecurityRoles>

  <!-- Define apps and the sitemap -->
  <ModelDrivenApps></ModelDrivenApps>

  <!-- Define how the app connects to the target CDS environment -->
  <CdsConnection></CdsConnection>

  <!-- Define pre-rolled logging outputs -->
  <LoggingConfiguration></LoggingConfiguration>

</ConfigurationManifest>
```

None of the above nodes are mandatory - either leave them blank or omit them entirely if they are not relevant to your requirements

The top level nodes are proccessed in the following order (or reverse order for deletions) to ensure components are created/updated in order of dependency

### OptionSets

```xml
<OptionSets>
  <OptionSet>
    <DisplayName>Laptop Model</DisplayName>
    <!--<SchemaName>new_laptopmodel</SchemaName>-->
    <!--<Items>-->
      <!--<Item>XPS 13</Item>-->
      <!--<Item>XPS 15</Item>-->
      <!--<Item>Surface Pro</Item>-->
      <!--<Item>Surface Go</Item>-->
      <!--<Item>Macbook Pro</Item>-->
    <!--</Items>-->
  </OptionSet>
</OptionSets>
```

### Security Roles

```xml
<SecurityRoles>
  <SecurityRole>
    <Name>Laptop Administrator</Name>
    <!--<Privileges>-->
      <!--<Privilege>prvExportToExcel</Privilege>-->
    <!--</Privileges>-->
  </SecurityRole>
</SecurityRoles>
```
N.B. On updates to an existing security role privileges are additive. Privileges removed from the manifest will not be removed from the role

Currently role privileges must either be manually removed or use the `clobber` flag to delete and recreate the role

### Entities

```xml
<Entities>
  <Entity>
    <DisplayName>Laptop</DisplayName>
    <!--<PluralName>Laptops</PluralName>-->
    <!--<SchemaName>new_laptop</SchemaName>-->
    <!--<Description>Stock of available laptops</Description>-->
    <!--<OwnershipType>UserOwned</OwnershipType>-->
    <!--<PrimaryAttributeName>Name</PrimaryAttributeName>-->
    <!--<PrimaryAttributeMaxLength>50</PrimaryAttributeMaxLength>-->
    <!--<PrimaryAttributeDescription>...</PrimaryAttributeDescription>-->
    <!--<IsActivity>false</IsActivity>-->
    <!--<HasActivities>false</HasActivities>-->
    <!--<HasNotes>true</HasNotes>-->
    <!--<IsQuickCreateEnabled>true</IsQuickCreateEnabled>-->
    <!--<IsAuditEnabled>true</IsAuditEnabled>-->
    <!--<IsDuplicateDetectionEnabled>false</IsDuplicateDetectionEnabled>-->
    <!--<IsBusinessProcessEnabled>false</IsBusinessProcessEnabled>-->
    <!--<IsDocumentManagementEnabled>false</IsDocumentManagementEnabled>-->
    <!--<IsValidForQueue>true</IsValidForQueue>-->
    <!--<ChangeTrackingEnabled>false</ChangeTrackingEnabled>-->
    <!--<NavigationColour></NavigationColour>-->
    <!-- **And others** -->
    <Attributes>
      <Attribute>
        <DisplayName>Laptop Make</DisplayName>
        <!--<SchemaName>new_laptopmake</SchemaName>-->
        <!--<DataType>String</DataType>-->
        <!--<Description>The make of the laptop</Description>-->
        <!--<RequiredLevel>ApplicationRequired</RequiredLevel>-->
        <!--<IsAuditEnabled>true</IsAuditEnabled>-->
        <!--<MaxLength>8</MaxLength>-->
        <!--<StringFormat>Text</StringFormat>-->
        <!--<AddToForm>true</AddToForm>-->
        <!--<AddToViewOrder>1</AddToViewOrder>-->
        <!-- **And others** -->
      </Attribute>
    </Attributes>
    <EntityPermissions>
      <Name>Laptop User</Name>
      <!--<Create>Deep</Create>-->
      <!--<Read>Deep</Read>-->
      <!--<Write>Local</Write>-->
      <!--<Delete>Basic</Delete>-->
      <!--<AppendTo>Deep</AppendTo>-->
      <!--<Append>Deep</Append>-->
      <!--<Share>Basic</Share>-->
    </EntityPermissions>
   </Entity>
 </Entities>
```

N.B. While `DisplayName` is the only mandatory node, if an entity or attribute is being updated, the schema name is derived from the Display Name plus the publisher of the named solution. So it is often wise to include the `SchemaName` if the manifest includes existing entities to prevent duplication

e.g. Assuming the 'EntityCreationSandbox' solution has a publisher with prefix of 'new', the below manifest would create a new entity 'new_contact'

```xml
<ConfigurationManifest>
  <SolutionName>EntityCreationSandbox</SolutionName>
  <Entities>
    <Entity>
      <DisplayName>Contact</DisplayName>
    </Entity>
  </Entities>
</ConfigurationManifest>
```

### ModelDrivenApps

```xml
<ModelDrivenApps>
  <ModelDrivenApp>
    <Name>Laptop Management</Name>
    <!--<UniqueName>new_laptopmanagement</UniqueName>-->
    <!--<Description>App for managing laptops and laptop reference data</Description>-->
    <!--<SiteMap>
      <Areas>
        <Area>
          <Name>New Area</Name>
          <Groups>
            <Group>
              <Name>New Group</Name>
              <SubAreas>
                <SubArea>
                  <Type>Entity</Type>
                  <Entity>new_laptop</Entity>
                  <Title>Laptops</Title>
                </SubArea>
              </SubAreas>
            </Group>
          </Groups>
        </Area>
      </Areas>
    </SiteMap>-->
  </ModelDrivenApp>
</ModelDrivenApps>
</ModelDrivenApps>
```

Note that while the `<SiteMap/>` node is not mandatory, it will not produce a functional app without one and would require manual intervention after generation

## Options

### Clobber

Delete everything defined or created in the manifest, change and re-build. Usually quicker than having to deal with certain settings that can't be changed. Does wipe data too though, so be careful!

N.B. Only gets rids of artifacts specified in the manifest so will fail if there are other dependencies created manually or externally to this manifest

  
