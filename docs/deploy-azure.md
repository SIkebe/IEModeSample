# Deployment Azure infrastructure

## Prerequisites
* [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
* Visual Studio Code
    * [Bicep](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-bicep)

## Deployments
1. Definitions
    ```bash
    resourceGroupName="ie-mode-sample-rg"
    location="japaneast"
    subscriptionId="95bb5196-0ee2-496b-a8cf-54b9598de127"
    ```

1. Login to Azure CLI
    ```bash
    az login
    az account set --subscription $subscriptionId
    ```

1. Create resource group
    ```bash
    az group create --name $resourceGroupName --location $location
    ```

1. Diff
    ```bash
    az deployment group what-if --resource-group $resourceGroupName --mode Complete --result-format FullResourcePayloads --template-file deploy/main.bicep
    ```

1. Deploy
    ```bash
    az deployment group create --resource-group $resourceGroupName --mode Complete --template-file deploy/main.bicep
    ```

1. Delete
    ```bash
    az group delete -n $resourceGroupName --yes
    ```
