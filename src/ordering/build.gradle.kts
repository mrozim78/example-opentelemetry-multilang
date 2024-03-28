import org.jetbrains.kotlin.gradle.tasks.KotlinCompile


plugins {
	id("org.springframework.boot") version "3.2.4"
	id("io.spring.dependency-management") version "1.1.4"
	id("io.ktor.plugin") version "2.3.9"
	kotlin("jvm") version "1.9.23"
	kotlin("plugin.spring") version "1.9.23"
}

group = "com.opentelemetry.kotlin.example"
version = "0.0.1-SNAPSHOT"

java {
	sourceCompatibility = JavaVersion.VERSION_21
}

application()
{
	mainClass.set("com.opentelemetry.kotlin.example.ordering.OrderingApplicationKt")
}

ktor()
{
	docker {
		localImageName="ordering"
		jreVersion.set(JavaVersion.VERSION_21)
		imageTag.set("1.0.0")
	}
}

repositories {
	mavenCentral()
}
dependencyManagement {
	imports {
		mavenBom("io.opentelemetry:opentelemetry-bom:1.36.0")
		mavenBom("io.opentelemetry.instrumentation:opentelemetry-instrumentation-bom-alpha:2.2.0-alpha")
	}
}

dependencies {
	implementation("org.springframework.boot:spring-boot-starter-web")
	implementation("org.springframework.boot:spring-boot-starter-mail")
	implementation("com.fasterxml.jackson.module:jackson-module-kotlin")
	implementation("org.jetbrains.kotlin:kotlin-reflect")
	testImplementation("org.springframework.boot:spring-boot-starter-test")
	implementation("io.opentelemetry.instrumentation:opentelemetry-spring-boot-starter")
	implementation("org.springframework.boot:spring-boot-starter-web");
	implementation("io.opentelemetry:opentelemetry-api");
	implementation("io.opentelemetry:opentelemetry-sdk");
	implementation("io.opentelemetry:opentelemetry-exporter-otlp:1.36.0");
	implementation("io.opentelemetry.semconv:opentelemetry-semconv:1.23.1-alpha");
	implementation("io.opentelemetry:opentelemetry-sdk-extension-autoconfigure");

}

tasks.withType<KotlinCompile> {
	kotlinOptions {
		freeCompilerArgs += "-Xjsr305=strict"
		jvmTarget = "21"
	}
}

tasks.withType<Test> {
	useJUnitPlatform()
}
