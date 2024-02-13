const amqplib = require('amqplib');




async function main(msg) {;

    const conn = await amqplib.connect("amqp://admin:admin@localhost");

    const ch1 = await conn.createChannel();

    const queue = 'new_task';

    ch1.assertQueue(queue);

    ch1.prefetch(1)

    ch1.sendToQueue(queue, Buffer.from(msg));

    setTimeout(function() {
        conn.close();
        process.exit(0)
        }, 500);
}


for (let i = 0; i < 6; i++) {
    setInterval(() => {
        main(`this is ${i}...`)
    }, 2000)              
}           