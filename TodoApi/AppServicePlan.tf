resource "azurerm_service_plan" "arServicePlan" {
  name                = "arServiceplanPub"
  resource_group_name = azurerm_resource_group.MinTodo-rg-AR.name
  location            = "West Europe"
  os_type             = "Linux"
  sku_name            = "B1"
}
