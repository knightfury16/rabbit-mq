const amqplib = require("amqplib");
const RPC_QUEUE = "rpc_queue";

async function GetFib(number){
    
    console.log(`Calculating Fibonacci for ${number}`)

    await new Promise(resolve => setInterval(resolve, 5000));
    
    return fib(number);
}

function fib(n){
    if(n <=1 ) return n;
    return fib(n-1) + fib( n - 2);
}

(async () => {

    var amqp = await amqplib.connect(URL = "amqp://admin:admin@localhost");

    var channel = await amqp.createChannel();

    await channel.assertQueue(RPC_QUEUE,{durable: false});

    channel.consume(RPC_QUEUE, async (msg) => {
        if(msg != null){
            var n = parseInt(msg.content.toString());
            var rpl_queue = msg.properties.replyTo;
            var correlationId = msg.properties.correlationId;

            console.log("Got an request for fib (%d)", n);

            var result = await GetFib(n);

            channel.sendToQueue(rpl_queue, Buffer.from(result.toString()),{correlationId})

            //acknowledge the message
            channel.ack(msg);
        }
        else{
            console.log("message is empty.");
        }
    });

})()
 
