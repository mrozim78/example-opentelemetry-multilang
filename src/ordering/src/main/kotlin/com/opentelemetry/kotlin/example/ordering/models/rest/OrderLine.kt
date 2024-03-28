package com.opentelemetry.kotlin.example.ordering.models.rest

data class OrderLine (
    val eventId:String,
    val ticketCount:Int,
    val price:Int
        )