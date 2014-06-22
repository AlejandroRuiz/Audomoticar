#include <Ultrasonic.h>
Ultrasonic UAdelante(5,4); // (Trig PIN,Echo PIN)
Ultrasonic UAtras(3,2);
int led1=29, led2=28, led3=27, led4=26, led5=25, led6=24, led7=23, led8=22;
String msg="c";
bool encendido=false;
int LUAdelante=0;
int LUAtras=0;
int mensaje=0;
int AGas=0;
void setup()
{
   pinMode(led1,OUTPUT);
   pinMode(led2,OUTPUT);
   pinMode(led3,OUTPUT);
   pinMode(led4,OUTPUT);
   pinMode(led5,OUTPUT);
   pinMode(led6,OUTPUT);
   pinMode(led7,OUTPUT);
   pinMode(led8,OUTPUT);
   Serial.begin(9600);
}
const unsigned long periodicMessageFrequency = 5000;//Frequency for periodic messages [milliseconds]
unsigned long time = 0;
void loop()
{
  unsigned long currentTime = millis();
	if ((currentTime - time) > periodicMessageFrequency)
	{
   AGas=analogRead(0);
		if(AGas>500)
                {
                  Serial.print("g");
                }
                if(AGas<500)
                {
                  Serial.print("3");
                }
		time = currentTime;
	}
/*if(AGas!=0)
  {
  }
  else
  {}*/
  LUAdelante = UAdelante.Ranging(CM);
  LUAtras = UAtras.Ranging(CM);
  if(LUAdelante<20 || LUAtras<20)
  {
  }
  else
  {
    mensaje=0;
    LUAdelante=0;
    LUAtras=0;
  }
  
  char character;
  while(Serial.available()) {
      msg="";
      character = Serial.read();
      msg.concat(character);
      if(msg=="1")
      {
        Serial.print("1");
        encendido = true;
      }
      if(msg=="2")
      {
        Serial.print("2");
        encendido = false;
      }
      if(msg=="a")
      {
        Serial.print("a");
      }
      if(msg=="b")
      {
        Serial.print("b");
      }
      if(msg=="c")
      {
        Serial.print("c");
      }
      if(msg=="q")
      {
        Serial.print("q");
      }
      if(msg=="l")
      {
        Serial.print("l");
      }
  }
  
  if(encendido== true)
  {
      if(msg=="a")
      {
           if(LUAdelante!=0 || LUAtras!=0)
    {
           if(mensaje==0)
     {
       Serial.print("x");
       mensaje=1;
     }
        digitalWrite(led1, LOW);
        digitalWrite(led2, HIGH);
        digitalWrite(led3, HIGH);
        digitalWrite(led4, LOW);
        digitalWrite(led5, LOW);
        digitalWrite(led6, LOW); 
        digitalWrite(led7, LOW);
        digitalWrite(led8, LOW);
    }
    else
    {
        digitalWrite(led1,HIGH);
        digitalWrite(led2, LOW);
        digitalWrite(led3,HIGH);
        digitalWrite(led4, LOW);
        digitalWrite(led5,HIGH);
        digitalWrite(led6, LOW);
        digitalWrite(led7,HIGH);
        digitalWrite(led8, LOW);
    }
      }
      if(msg=="b")
      {
           if(LUAdelante!=0 || LUAtras!=0)
    {
           if(mensaje==0)
     {
       Serial.print("x");
       mensaje=1;
     }
        digitalWrite(led1, LOW);
        digitalWrite(led2, HIGH);
        digitalWrite(led3, HIGH);
        digitalWrite(led4, LOW);
        digitalWrite(led5, LOW);
        digitalWrite(led6, LOW); 
        digitalWrite(led7, LOW);
        digitalWrite(led8, LOW);
    }
    else
    {
        digitalWrite(led1, LOW);
        digitalWrite(led2, HIGH);
        digitalWrite(led3, LOW);
        digitalWrite(led4, HIGH);
        digitalWrite(led5, LOW);
        digitalWrite(led6, HIGH); 
        digitalWrite(led7, LOW);
        digitalWrite(led8, HIGH);
    }
      }
      if(msg=="c")
      {
        digitalWrite(led1, LOW);
        digitalWrite(led2, LOW);
        digitalWrite(led3, LOW);
        digitalWrite(led4, LOW);
        digitalWrite(led5, LOW);
        digitalWrite(led6, LOW);
        digitalWrite(led7, LOW);
        digitalWrite(led8, LOW);
      }
      if(msg=="q")
      {
        digitalWrite(led1, LOW);
        digitalWrite(led2, HIGH);
        
        digitalWrite(led3, HIGH);
        digitalWrite(led4, LOW);
        
        digitalWrite(led5, LOW);
        digitalWrite(led6, LOW); 
        
        digitalWrite(led7, LOW);
        digitalWrite(led8, LOW);
      }
      if(msg=="l")
      {
        digitalWrite(led1, HIGH);
        digitalWrite(led2, LOW);
        
        digitalWrite(led3, LOW);
        digitalWrite(led4, HIGH);
        
        digitalWrite(led5, LOW);
        digitalWrite(led6, LOW); 
        
        digitalWrite(led7, LOW);
        digitalWrite(led8, LOW);
      }
    }
    else
    {
      digitalWrite(led1, LOW);
      digitalWrite(led2, LOW);
      digitalWrite(led3, LOW);
      digitalWrite(led4, LOW);    
      digitalWrite(led5, LOW);
      digitalWrite(led6, LOW);
      digitalWrite(led7, LOW);
      digitalWrite(led8, LOW);
    }
}
