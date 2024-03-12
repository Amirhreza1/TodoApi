resource "azurerm_cosmosdb_account" "CodmosDbex" {
  name                = "min-cosmosdb-konto"
  location            = "West Europe"
  resource_group_name = azurerm_resource_group.MinTodo-rg-AR.name
  offer_type          = "Standard"
  kind                = "GlobalDocumentDB"
  
  consistency_policy {
    consistency_level = "Session"
  }

  geo_location {
    location          = "West Europe"
    failover_priority = 0
  }
    
  capabilities {
    name = "EnableServerless"
  }
}
