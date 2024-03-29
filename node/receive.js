const amqplib = require('amqplib');


(async () => {

    const conn = await amqplib.connect('amqp://admin:admin@localhost')

    const ch2 = await conn.createChannel();

    const queue = 'hello';

    await ch2.assertQueue(queue, { durable: false, exclusive: false, autoDelete: false, arguments: null })

    ch2.consume(queue, (msg) => {
        if (msg !== null) {
            console.log(msg.content.toString());
            ch2.ack(msg)
        } else {
            console.log('consumer cancled by server')
        }
    })

})()