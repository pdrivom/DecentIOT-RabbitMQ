
# DecentIOT-RabbitMQ

This is the [DecentIOT-RabbitMQ ](https://www.nuget.org/packages/DecentIOT.RabbitMQ.Client/) Nuget Package  repository. The source code includes Unit Testing that can run after the [RabbitMQ](https://www.rabbitmq.com/download.html) Server being installed.

This is a [RabbitMQ.Client](https://www.nuget.org/packages/RabbitMQ.Client) Nuget Package wrapper. It eases the usage and creates a nice abstraction. The Request/Response patterns is also implemented.

This package belongs to a bigger project named DecentIOT.


To support the usage of this package, be sure to have the all the knowledge needed, consulting the official [RabbitMQ Documentation](https://www.rabbitmq.com/tutorials/amqp-concepts.html).

## Usage

### Client

Firstly the client to the RabbitMQ server needs to be created.

The zero arguments constructor has as optional all the parameters needed to start on a newly RabbitMQ instalation.

```csharp
using RabbitMQ.Client;
using DecentIOT.RabbitMQ;


var client = new RabbitClient();
```

Below it can be found the way to use the other parameters.
```csharp
using RabbitMQ.Client;
using DecentIOT.RabbitMQ;


var client = RabbitClient("localhost","/",5672,"guest","guest");
```

### Exchanges, Queues, Producers and Consumers

> **Exchanges** are AMQP 0-9-1 entities where messages are sent to. Exchanges take a message and route it into zero or more queues. The
> routing algorithm used depends on the _exchange type_ and rules called
> _bindings_.
> 
> The term "**publisher**" means different things in different contexts.
> In general in messaging a publisher (also called "producer") is an
> application (or application instance) that publishes (produces)
> messages.
> 
> The term "**consumer"** means different things in different contexts.
> In general in messaging a consumer is an application (or application
> instance) that consumes messages.

The all point of this package is to simplify the usage of the client, so each **RabbitConsumer** Object is binded with **only** one **Queue**.

## Direct Exchange
A direct exchange delivers messages to queues **based on the message routing key**. A direct exchange is ideal for the unicast routing of messages (although they can be used for multicast routing as well).
```csharp
using RabbitMQ.Client;
using DecentIOT.RabbitMQ;
using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Queue;


var client = new RabbitClient();

var directExchange = Client.CreateDirectExchange("ex.direct");

var directQueue = DirectExchange.CreateQueue("direct.animal.queue", "duck.wild.life");
```
#### Producer
Here the producer sends a batch of messages to the ***routing key*** binded to the Queue.
```csharp
using DecentIOT.RabbitMQ.Producer;


var producer = Client.CreateProducer(directExchange);

var messages = new List<RabbitMessage>
{
    new RabbitMessage("duck.wild.life", "quack!"),
    new RabbitMessage("duck.wild.life", "oh shit!"),
    new RabbitMessage("duck.wild.life", "a wolf!")
};
producer.PublishMany(messages);

```
#### Synchronous Consumer (Pull)
Here the messages are pulled (dequeued) from the created ***direct.animal.queue***.
```csharp
using DecentIOT.RabbitMQ.Consumer;


var consumer = Client.CreateConsumer(directQueue);

var duck1Message = consumer.PullMessage(); //"quack!"
var duck2Message = consumer.PullMessage(); //"oh shit!"
var duck3Message = consumer.PullMessage(); //"a wolf!"

```
#### Asynchronous Consumer 
Here the consumer waits for a new message pushed to the Exchange and routed to\ ***direct.animal.queue***.
```csharp
using DecentIOT.RabbitMQ.Consumer;


var consumer = Client.CreateConsumer(directQueue);

consumer.NewMessage += (consumer, queue, message) =>
{
	//"quack!"
	//"oh shit!"
	//"a wolf!"
};
consumer.StartConsuming();
```

## Fanout Exchange
A fanout exchange routes messages to all of the queues that are bound to it and the **routing key is ignored**. If N queues are bound to a fanout exchange, when a new message is published to that exchange a copy of the message is delivered to all N queues. Fanout exchanges are ideal for the broadcast routing of messages
```csharp
using RabbitMQ.Client;
using DecentIOT.RabbitMQ;
using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Queue;


var client = new RabbitClient();

var fanoutExchange = Client.CreateFanoutExchange("ex.fanout");

var queue1 = fanoutExchange .CreateQueue("fanout.animal.queue1"));
var queue2 = fanoutExchange .CreateQueue("fanout.animal.queue2"));
```
#### Producer
Here the producer sends one  messages to the Exchange.
```csharp
using DecentIOT.RabbitMQ.Producer;


var producer = Client.CreateProducer(fanoutExchange);

producer.PublishSingle(new RabbitMessage("", "quack!"));

```
#### Synchronous Consumer (Pull)
Here the messages are pulled (dequeued) from the created ***fanout.animal.queue1*** and ***fanout.animal.queue2*** with only one message was pushed .
```csharp
using DecentIOT.RabbitMQ.Consumer;


var consumer1 = Client.CreateConsumer(queue1);
var consumer2 = Client.CreateConsumer(queue2);

var duck1Message = consumer.PullMessage(); //"quack!"
var duck2Message = consumer.PullMessage(); //"quack!"

```
#### Asynchronous Consumer 
Here the consumers waits for a new message pushed to the Exchange then routed to ***fanout.animal.queue1*** and ***fanout.animal.queue2***.
```csharp
using DecentIOT.RabbitMQ.Consumer;


var consumer1 = Client.CreateConsumer(queue1);
var consumer2 = Client.CreateConsumer(queue2);

consumer1.NewMessage += (consumer, queue, message) =>
{
	//"quack!"
};
consumer2.NewMessage += (consumer, queue, message) =>
{
	//"quack!"
};
consumer1.StartConsuming();
consumer2.StartConsuming();
```
## Headers Exchange
A headers exchange is designed for routing on multiple attributes that are more easily expressed as message headers than a routing key. Headers exchanges **ignore the routing key attribute**. Instead, the **attributes used for routing** are taken from the headers attribute. 
```csharp
using RabbitMQ.Client;
using DecentIOT.RabbitMQ;
using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Queue;


var client = new RabbitClient();

var headersExchange = Client.CreateHeadersExchange("ex.headers");

var queue1 = headersExchange.CreateQueue("headers.animal.queue1",x_match.any,new List<RabitMessageHeader> { new RabitMessageHeader("animal","duck"),new RabitMessageHeader("color","white")}));
var queue2 = headersExchange.CreateQueue("headers.animal.queue2",x_match.all,new List<RabitMessageHeader> { new RabitMessageHeader("animal", "dog"), new RabitMessageHeader("color", "white") }));
```
#### Producer
Here the producer sends a messages to the Exchange with the specified headers.
```csharp
using DecentIOT.RabbitMQ.Producer;


var producer = Client.CreateProducer(headersExchange);

var message = new RabbitMessage("", "quack!");
message.AddHeader("color","white");

producer.PublishSingle(message);
```
#### Synchronous Consumer (Pull)
Here the messages are pulled (dequeued) from the created ***headers.animal.queue1***, but not from ***headers.animal.queue2*** since this queue needs to match all headers attributes .
```csharp
using DecentIOT.RabbitMQ.Consumer;


var consumer1 = Client.CreateConsumer(queue1);
var consumer2 = Client.CreateConsumer(queue2);

var queue1Message = consumer.PullMessage(); //"quack!"
var queue2Message = consumer.PullMessage(); //no message on queue, must match all arguments
```
#### Asynchronous Consumer 
Here the consumers waits for a new message pushed to the Exchange then routed to ***headers.animal.queue1*** and ***headers.animal.queue2***.
```csharp
using DecentIOT.RabbitMQ.Consumer;


var consumer1 = Client.CreateConsumer(queue1);
var consumer2 = Client.CreateConsumer(queue1);

consumer1.NewMessage += (consumer, queue, message) =>
{
	//"quack!"
};
consumer2.NewMessage += (consumer, queue, message) =>
{
	//no message on queue
};
consumer1.StartConsuming();
consumer2.StartConsuming();
```
## Topic
Topic exchanges route messages to one or many queues based on **matching between a message routing key and the pattern that was used to bind a queue** to an exchange. The topic exchange type is often used to implement various publish/subscribe pattern variations. 
```csharp
using RabbitMQ.Client;
using DecentIOT.RabbitMQ;
using DecentIOT.RabbitMQ.Exchange;
using DecentIOT.RabbitMQ.Queue;


var client = new RabbitClient();

var topicExchange = Client.CreateTopicExchange("ex.topic");
var topicQueue = TopicExchange.CreateQueue("topic.animal.queue", "*.wild.*");
```
#### Producer
Here the producer sends a batch of messages to the Exchange and each contains a ***routing key*** .
```csharp
using DecentIOT.RabbitMQ.Producer;


var producer = Client.CreateProducer(topicExchange);
 var messages = new List<RabbitMessage>
{
	new RabbitMessage("duck.wild.life", "quack!"),
	new RabbitMessage("dog.wild.dream", "woof!"),
	new RabbitMessage("cat.wild.attitude", "miau!")
};
producer.PublishMany(messages);
```
#### Synchronous Consumer (Pull)
Here all the messages are pulled (dequeued) from the created ***topic.animal.queue*** since all the routing keys contain the word ***wild*** as binded to the wildcard.
```csharp
using DecentIOT.RabbitMQ.Consumer;


var consumer = Client.CreateConsumer(topicQueue);

var duckMessage = consumer.PullMessage(); //"quack!"
var dogMessage = consumer.PullMessage(); //"woof!"
var catMessage = consumer.PullMessage(); //"miau!"
```
#### Asynchronous Consumer 
In this example the consumers waits for a new message pushed to the Exchange then routed to ***topic.animal.queue***.
```csharp
using DecentIOT.RabbitMQ.Consumer;


var consumer = Client.CreateConsumer(topicQueue);

consumer.NewMessage += (consumer, queue, message) =>
{
	//"quack!"
	//"woof!"
	//"miau!"
};
consumer.StartConsuming();
```
## Request/Response
The request/response pattern between two parties is described as: the requester sends a request to the responder, the responder starts the work and sends the response to the requester once the work is done.

#### Synchronous Requester
In this example the requester sends a request, that can be of any type and waits some time then pulls the response.
```csharp
using DecentIOT.RabbitMQ.Requester;


var requester = Client.CreateRequester("ex.reqresp", "animals");
requester.SendRequest("dogs");

//Wait some time

var response = requester.GetResponse();
```

#### Synchronous Responder
In this example the responder pulls a request, processes it and then send the response.
```csharp
using DecentIOT.RabbitMQ.Responder;


var responder = Client.CreateResponser("ex.reqresp", "animals");

var request = Responder.GetRequest();
if(request.Content == "dogs")
{
	responder.SendResponse(request, new List<string> { "Poodle", "German Shepard", "Border Colie" });
}
```
#### Asynchronous Requester
In this example the requester sends a request, that can be of any type and when the responder sends the response the event ***NewResponse***  will be fired.
```csharp
using DecentIOT.RabbitMQ.Requester;


var requester = Client.CreateRequester("ex.reqresp", "animals");

requester.NewResponse += (requester, response) =>
{
    Console.WriteLine(response.Content);
};

requester.SendRequest("dogs");
```

#### Asynchronous Responder
In this example the responder is listening for a request, when it arrives, it will be  processed and then response sent.
```csharp
using DecentIOT.RabbitMQ.Responder;


var responder = Client.CreateResponser("ex.reqresp", "animals");

responder.NewRequest += (request) =>
{
    if (request.Content == "dogs")
    {
        Responder.SendResponse(request, new List<string> { "Poodle", "German Shepard", "Border Colie" });
    }
};
```