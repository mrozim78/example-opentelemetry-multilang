package com.opentelemetry.kotlin.example.ordering.models.rest

data class CustomerDetails (
    val name:String,
    val email:String,
    val address:String,
    val town:String,
    val postalCode:String,
    val creditCardNumber:String,
    val creditCardExpiryDate:String
    )