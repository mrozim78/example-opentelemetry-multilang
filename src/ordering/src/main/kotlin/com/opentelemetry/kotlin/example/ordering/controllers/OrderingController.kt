package com.opentelemetry.kotlin.example.ordering.controllers

import com.opentelemetry.kotlin.example.ordering.models.rest.OrderForCreation
import com.opentelemetry.kotlin.example.ordering.services.OrderingService
import io.opentelemetry.api.OpenTelemetry
import io.opentelemetry.api.trace.Tracer
import org.slf4j.LoggerFactory
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.web.bind.annotation.PutMapping
import org.springframework.web.bind.annotation.RequestBody
import org.springframework.web.bind.annotation.RestController


@RestController
class OrderingController(val orderingService: OrderingService,val openTelemetry: OpenTelemetry)
{
    private var tracer: Tracer? = null

    @Autowired
    fun RollController() {
        tracer = openTelemetry.getTracer(OrderingController::class.java.getName(), "0.1.0")
    }
    private val logger = LoggerFactory.getLogger(OrderingController::class.simpleName)
    @PutMapping(path = ["/ordering"])
    fun ordering(@RequestBody orderForCreation: OrderForCreation )
    {
        logger.info("Get order")
        val parentSpan = tracer!!.spanBuilder("parent").startSpan()
        parentSpan.addEvent("Start ordering")
        orderingService.ordering(orderForCreation)
        parentSpan.addEvent("End ordering")
        parentSpan.end();
    }
}