import os
import datetime

import logging
from flask import Flask, jsonify
from os import environ


#Flask
from flask import Flask
from opentelemetry.instrumentation.flask import FlaskInstrumentor
from opentelemetry.instrumentation.wsgi import collect_request_attributes

#OpenTelemetry resources
from opentelemetry.sdk.resources import SERVICE_NAME, SERVICE_INSTANCE_ID, Resource

#OpenTelemetry traces
from opentelemetry.trace import set_tracer_provider, get_current_span,  SpanKind
from opentelemetry.sdk.trace import TracerProvider
from opentelemetry.exporter.otlp.proto.grpc.trace_exporter import OTLPSpanExporter
from opentelemetry.sdk.trace.export import BatchSpanProcessor

#OpenTelemetry logs
from opentelemetry._logs import set_logger_provider
from opentelemetry.exporter.otlp.proto.grpc._log_exporter import OTLPLogExporter
from opentelemetry.instrumentation.logging import LoggingInstrumentor
from opentelemetry.sdk._logs import LoggerProvider, LoggingHandler 
from opentelemetry.sdk._logs.export import BatchLogRecordProcessor

#OpenTelemetry metrics
from opentelemetry.metrics import set_meter_provider
from opentelemetry.instrumentation.system_metrics import SystemMetricsInstrumentor
from opentelemetry.sdk.metrics import MeterProvider
from opentelemetry.exporter.otlp.proto.grpc.metric_exporter import OTLPMetricExporter
from opentelemetry.sdk.metrics.export import PeriodicExportingMetricReader

# Read variables
HOST = os.environ.get("PROGRAMATIC_HOST","catalog" )
EXPOSE_PORT = int(os.environ.get("PROGRAMATIC_EXPOSE_PORT", 8002))
OTLP_ENDPOINT = os.environ.get("PROGRAMATIC_OTLP_ENDPOINT", "http://otel-collector:4317")
OTLP_SERVICE_NAME = os.environ.get("PROGRAMATIC_SERVICE_NAME", "catalog")
OTLP_SERVICE_INSTANCE_ID = os.environ.get("PROGRAMATIC__SERVICE_INSTANCE_ID", "catalog-instance-1")


# Flask instrumentator
instrumentor = FlaskInstrumentor()
LoggingInstrumentor()


app = Flask(__name__)

# Resource OpenTelemetry
resource = Resource(attributes={
    SERVICE_NAME: OTLP_SERVICE_NAME,
    SERVICE_INSTANCE_ID: OTLP_SERVICE_INSTANCE_ID
})


# Traces
provider = TracerProvider(resource=resource)
processor = BatchSpanProcessor(OTLPSpanExporter(endpoint=OTLP_ENDPOINT))
provider.add_span_processor(processor)

set_tracer_provider(provider)
tracer = provider.get_tracer(__name__)
instrumentor.instrument_app(app=app , tracer_provider=provider)


#Logs
LoggingInstrumentor().instrument(set_logging_format=True)
logger_provider = LoggerProvider(resource=resource)
set_logger_provider(logger_provider)

logger_provider.add_log_record_processor(BatchLogRecordProcessor(OTLPLogExporter(endpoint=OTLP_ENDPOINT)))
handler = LoggingHandler(level=logging.NOTSET, logger_provider=logger_provider)

logger=logging.getLogger("programatic")
logger.addHandler(handler)


# Metrics
configurationMetrics = {
    "system.memory.usage": ["used", "free", "cached"],
    "system.cpu.time": ["idle", "user", "system", "irq"],
    "process.runtime.memory": ["rss", "vms"],
    "process.runtime.cpu.time": ["user", "system"]
}

meter_provider = MeterProvider(resource=resource, metric_readers=[PeriodicExportingMetricReader(OTLPMetricExporter(endpoint=OTLP_ENDPOINT))])
set_meter_provider(meter_provider)
SystemMetricsInstrumentor(config=configurationMetrics).instrument()



def example_data():
    logging.error("Example data catalog")
    now = datetime.datetime.now()
    days7 = now + datetime.timedelta(days=7)
    day3 = now + datetime.timedelta(days=3)
    days5 = now + datetime.timedelta(days=5)
    data = [
        {'EventId': 'CFB88E29-4744-48C0-94FA-B25B92DEA317', 'Name': 'ABBA concert', 'Price': 100, 'Artist': 'ABBA',
         'Date': days7.isoformat(), 'Description': 'ABB concert', 'ImageUrl': ''},
        {'EventId': 'CFB88E29-4744-48C0-94FA-B25B92DEA318', 'Name': 'Reload', 'Price': 300, 'Artist': 'Metallica',
         'Date': days5.isoformat(), 'Description': 'Battery', 'ImageUrl': ''},
        {'EventId': 'CFB88E29-4744-48C0-94FA-B25B92DEA319', 'Name': 'Thunder', 'Price': 150, 'Artist': 'AC/DC',
         'Date': day3.isoformat(), 'Description': 'Thunder Concert', 'ImageUrl': ''}
    ]
    return data


@app.route("/event")
def events():
    with tracer.start_as_current_span(
       name="Run all event method",
       kind=SpanKind.INTERNAL
    ) as parent :
        logger.error("All Event method run")
        parent.add_event("Start all event method")      
        parent.add_event("End all event method")
        get_current_span()
    
    return jsonify(example_data())


@app.route("/event/<event_id>")
def event(event_id):
    with tracer.start_as_current_span(
       name="Run event method",
       kind=SpanKind.INTERNAL
    ) as parent :
        logger.error("Event method run")
        parent.add_event("Start event method")      
        parent.add_event("End event method")
        get_current_span()
    data = example_data()
    data_filter = list(filter(lambda x: (x['EventId'] == event_id.upper()), data))
    return jsonify(data_filter[0])


app.run(host="catalog", port=EXPOSE_PORT)
