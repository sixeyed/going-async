# going-async

Demo code for the Webinar: 'Scaling with Asynchronous Messaging'.

The Webinar was hosted by [@ParticularSW](http://particular.net/), the makers of NServiceBus, ServiceMatrix, ServicePulse etc.

It was presented on 21st April 2015 by me, [@EltonStoneman](http://geekswithblogs.net/eltonstoneman/Default.aspx), with help from @DanielMarbach and @MauroServienti.

# The Code

Looks at a very simple Validate-Enrich problem with three different solution approaches:

+ v1.0 - synchronous, sequential processing in a single host
+ v1.1 & v1.2 - parallel processing in a single host
+ v2.0 - parallel processing across many hosts

All versions use SQL Server (configs expect a local, un-named instance).

For the distributed version, the solution uses MSMQ for the message queues.

So this is for Windows only.

## Usage

*setup.cmd* to create the queues & input directories. 

Build the solution, and publish the database project, then *run-app-v1.x.cmd* to generate some input files and process them. 

*teardown.cmd* to remove the queues and the input directory.

## Next Steps

The Webinar focuses on how to scale your system, but there's much more to messaging. 

Check out my @Pluralsight course, [Message Queue Fundamentals in .NET](http://www.pluralsight.com/courses/message-queue-fundamentals-dotnet) for more patterns, practices, and lots more queueing technologies.

