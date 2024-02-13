const amqplib = require('amqplib');


(async () => {
    const conn = await amqplib.connect("amqp://admin:admin@localhost");

    const ch1 = await conn.createChannel();

    const queue = 'new_task';

    ch1.assertQueue(queue);

    ch1.consume(queue, (msg) => {
        if(msg != null){
            
            var secs = msg.content.toString().split('.').length - 1;

            
            console.log(" [x] Received %s", msg.content.toString());
            
            setTimeout(function() {
                console.log(" [x] Done");
                ch1.ack(msg);
            }, secs * 1000);

        }else{
            console.log("msg is null!")
        }
    })
})()