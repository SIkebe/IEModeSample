param location string = resourceGroup().location

param appNamePrefix string = uniqueString(resourceGroup().id)
var appServicePlanName = '${appNamePrefix}-appServicePlan'
var webAppName = 'ie-mode-sample-webapp'
var logAnalyticsWorkSpaceName = '${appNamePrefix}-loganalytics'
var appInsightsName = '${appNamePrefix}-appinsights'

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'F1'
  }
}

resource appService 'Microsoft.Web/sites@2022-03-01' = {
  name: webAppName
  location: location
  dependsOn: [
    logAnalyticsWorkSpace
  ]
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      minTlsVersion: '1.2'
    }
  }

  resource appServiceAppSettings 'config' = {
    name: 'appsettings'
    properties: {
      APPINSIGHTS_INSTRUMENTATIONKEY: appInsights.properties.InstrumentationKey
      ApplicationInsightsAgent_EXTENSION_VERSION: '~2' // Enable Application Insights
      XDT_MicrosoftApplicationInsights_Mode: 'default'
    }
  }

  resource appServiceMetadataConfig 'config' = {
    name: 'metadata'
    properties: {
      CURRENT_STACK: 'dotnet'
    }
    dependsOn: [
      appServiceAppSettings
    ]
  }

  resource appServiceWebConfig 'config' = {
    name: 'web'
    properties: {
      netFrameworkVersion: 'v6.0'
    }
    dependsOn: [
      appServiceMetadataConfig
    ]
  }

  resource appServiceLogConfig 'config' = {
    name: 'logs'
    dependsOn: [
      appServiceWebConfig
    ]
    properties: {
      applicationLogs: {
        fileSystem: {
          level: 'Information'
        }
      }
      httpLogs: {
        fileSystem: {
          enabled: true
          retentionInDays: 30
          retentionInMb: 35
        }
      }
      failedRequestsTracing: {
        enabled: true
      }
      detailedErrorMessages: {
        enabled: true
      }
    }
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkSpace.id
  }
}

resource logAnalyticsWorkSpace 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' = {
  name: logAnalyticsWorkSpaceName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
    features: {
      searchVersion: 1
      legacy: 0
      enableLogAccessUsingOnlyResourcePermissions: true
    }
  }
}
