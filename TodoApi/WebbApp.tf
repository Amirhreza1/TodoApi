resource "azurerm_linux_web_app" "MinTodoAppAR" {
  name                = "TodoAppAR"
  location            = "West Europe"
  resource_group_name = azurerm_resource_group.MinTodo-rg-AR.name
  service_plan_id = azurerm_service_plan.arServicePlan.id

  site_config {}
}
