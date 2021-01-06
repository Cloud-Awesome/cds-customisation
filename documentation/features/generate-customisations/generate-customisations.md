# Generate Customisations

> API and Command line application which processes an XML manifest to programatically create:
> - Entities, attributes, forms, views and global optionsets
> - Model driven apps and sitemaps
> - Security roles and field level profiles
> 
> Quickly generate, test and tear down data model and UI artifacts during prototyping phases, save time and prevent typos when generating large data models. Or integrate into an existing solution to migrate or synchronise data model from a 3rd party application into CDS

## Manifest

### XSD 
(Reference the version available in Blob Storage)

### Entities

```xml
<Entity>
  <DisplayName>Laptop</DisplayName>
  <!--<PluralName>Laptops</PluralName>-->
  <!--<SchemaName>awe_laptop</SchemaName>-->
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
      <!--<SchemaName>awe_laptopmake</SchemaName>-->
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
```

### OptionSets

### Security Roles

### ModelDrivenApps

###

## Options

### Clobber

Delete everything defined or created in the manifest, change and re-build. Usually quicker than having to deal with certain settings that can't be changed. Does wipe data too though, so be careful!

Also, only gets rids of artifacts specified in the manifest so will fail if there are other dependencies created manually or externally to this manifest

  
