const amqplib = require('amqplib');


async function main(msg) {;

    const conn = await amqplib.connect("amqp://admin:admin@localhost");

    const ch1 = await conn.createChannel();

    const exchange = 'logs';

    await ch1.assertExchange(exchange,'fanout',{
      durable: false
    })

    ch1.publish(exchange,"",Buffer.from(msg))

    setTimeout(function() {
        conn.close();
        process.exit(0)
        }, 500);
}

main("System crashed!!!!")