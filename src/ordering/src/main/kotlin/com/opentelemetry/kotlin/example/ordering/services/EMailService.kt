package com.opentelemetry.kotlin.example.ordering.services
import org.slf4j.LoggerFactory
import org.springframework.mail.SimpleMailMessage
import org.springframework.mail.javamail.JavaMailSender
import org.springframework.stereotype.Service

@Service
class EMailService (val emailSender: JavaMailSender){
    private val logger = LoggerFactory.getLogger(EMailService::class.simpleName)

    fun sendEMail(email: SimpleMailMessage) {
        try{

            logger.info("Try to send email")
            logger.info("EmailFrom:${email.from}")
            logger.info("EmailTo:${email.to}")
            logger.info("Subject:${email.subject}")
            logger.info("Body:${email.text}")
            emailSender.send(email)
        } catch (e:Exception)
        {
            logger.error("Send email error",e)
        }
    }
}