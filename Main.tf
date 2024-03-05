provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "example" {
  name     = "Todo-rg-AR"
  location = "West Europe"
}
