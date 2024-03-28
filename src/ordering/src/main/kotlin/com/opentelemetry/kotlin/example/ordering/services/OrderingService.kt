package com.opentelemetry.kotlin.example.ordering.services

import com.opentelemetry.kotlin.example.ordering.models.rest.OrderForCreation
import org.slf4j.LoggerFactory
import org.springframework.mail.SimpleMailMessage
import org.springframework.stereotype.Service

@Service
class OrderingService (val eMailService:EMailService)
{
    private val logger = LoggerFactory.getLogger(EMailService::class.simpleName)
    fun ordering(order: OrderForCreation)
    {
        logger.info("Ordering")
        logger.info("Build mail")
        val subject="Thank you for your order"
        val emailFrom ="shop.event@mail.com"
        val emailTo ="client@client.com"
        val totalSumPrice:Int = order.lines.sumOf { a->a.price*a.ticketCount }
        val totalSumTickets:Int = order.lines.sumOf { a->a.ticketCount }
        val body = "Thank you ${order.customerDetails.name}. You buy $totalSumTickets tickets on total price $$totalSumPrice. Your your order is being processed."
        val eMail = SimpleMailMessage()
        eMail.from=emailFrom
        eMail.setTo(emailTo)
        eMail.subject = subject
        eMail.text = body
        logger.info("Send mail")
        eMailService.sendEMail(eMail)
    }
}