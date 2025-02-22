namespace Learning.Shared.Dto.ModelExams.Payment;

public class ModelExamPaymentReceipt
{
    public required DateTimeOffset OrderPurchasedOn { get; set; }
    public required string OrderReferenceId { get; set; }
    public required string SoldTo { get; set; }
    public required string Item { get; set; }
    public required float ItemAmount { get; set; }
    public required float GstAmount { get; set; }
    public required int Gst { get; set; }
    public required float TotalPaid { get; set; }

}
