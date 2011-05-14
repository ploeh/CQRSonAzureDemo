This is a more updated and polished version of the demo code accompanying the MSDN Magazine article CQRS on Azure (http://msdn.microsoft.com/en-us/magazine/gg983487.aspx).

To enable sending of email you must edit the BookingCloudService\ServiceConfiguration.cscfg to configure an SMTP server.

These are the settings which should be configured:

      <Setting name="SmtpServerAddress" value="your.smtpserver.here" />
      <Setting name="SmtpUserName" value="" />
      <Setting name="SmtpPassword" value="" />
      <Setting name="EmailSenderSmtpAddress" value="you.email@address.here" />

As this is Windows Azure demo code, you must have the Windows Azure SDK installed in Visual Studio 2010 in order to compile.