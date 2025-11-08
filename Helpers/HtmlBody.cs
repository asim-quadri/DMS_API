namespace ComplianceAPI.Helpers
{
    public static class HtmlBody
    {
        public static readonly string RequestReceivedSubject = "We Have Received Your Request";

        public static readonly string RequestReceived = @"
<!DOCTYPE html>
<html>
<head>
  <meta charset='UTF-8'>
  <title>Request Received</title>
</head>
<body style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px; color: #333;'>
  <table width='100%' cellpadding='0' cellspacing='0' style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>
    <tr>
      <td style='padding: 20px;'>
        <h2 style='color: #2c3e50;'>Hi <strong>{0}</strong>,</h2>
        <p>We have received your request. Please allow us up to <strong>24 hours</strong> to revert back to you.</p>
        <p>If you have any urgent queries, feel free to contact our support team.</p>
        <br>
        <p>Regards,<br><strong>{1}</strong></p>
      </td>
    </tr>
  </table>
</body>
</html>";
    }
}