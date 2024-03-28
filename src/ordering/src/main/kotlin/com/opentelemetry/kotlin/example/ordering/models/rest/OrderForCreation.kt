package com.opentelemetry.kotlin.example.ordering.models.rest

import java.time.LocalDate

data class OrderForCreation(
    val orderId:String,
    val date:String,
    val customerDetails: CustomerDetails,
    val lines:List<OrderLine>
    )



