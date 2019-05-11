#region Imports
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stripe;
#endregion

namespace AzureFunctionsLabs.HTTPTrigger
{
    public static partial class HTTPFunctions
    {
        [FunctionName("StripeWebhook")]
        public static async Task<IActionResult> StripeWebhook(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                // read stripe webhook secret form appsettings
                var stripeWebhookSecret = Environment.GetEnvironmentVariable("StripeWebhookSecret");

                // read stripe signature form header
                var stripeSignature = req.Headers["Stripe-Signature"];


                var json = await new StreamReader(req.Body).ReadToEndAsync();

                // validate webhook called by stripe only
                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, stripeWebhookSecret);

                switch (stripeEvent.Type)
                {
                    case "customer.created":
                        var customer = stripeEvent.Data.Object as Customer;
                        // do work

                        break;

                    case "customer.subscription.created":
                    case "customer.subscription.updated":
                    case "customer.subscription.deleted":
                    case "customer.subscription.trial_will_end":
                        var subscription = stripeEvent.Data.Object as Subscription;
                        // do work

                        break;

                    case "invoice.created":
                        var newinvoice = stripeEvent.Data.Object as Invoice;
                        // do work

                        break;

                    case "invoice.upcoming":
                    case "invoice.payment_succeeded":
                    case "invoice.payment_failed":
                        var invoice = stripeEvent.Data.Object as Invoice;
                        // do work

                        break;

                    case "coupon.created":
                    case "coupon.updated":
                    case "coupon.deleted":
                        var coupon = stripeEvent.Data.Object as Coupon;
                        // do work

                        break;

                    // DO SAME FOR OTHER EVENTS
                }

                return new OkObjectResult("");
            }
            catch (StripeException ex)
            {
                log.LogError(ex, $"StripWebhook: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"StripWebhook: {ex.Message}");
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }


    public static class EventsConstants
    {
        //
        // Summary:
        //     Occurs whenever an account status or property has changed.
        public const string AccountUpdated = "account.updated";
        //
        // Summary:
        //     Occurs whenever a new payout is created.
        public const string PayoutCreated = "payout.created";
        //
        // Summary:
        //     Occurs whenever a payout is canceled.
        public const string PayoutCanceled = "payout.canceled";
        //
        // Summary:
        //     Occurs whenever an order return is created.
        public const string OrderReturnCreated = "order_return.created";
        //
        // Summary:
        //     Occurs whenever an order is updated.
        public const string OrderUpdated = "order.updated";
        //
        // Summary:
        //     Occurs whenever payment is attempted on an order, and the payment succeeds.
        public const string OrderPaymentSucceeded = "order.payment_succeeded";
        //
        // Summary:
        //     Occurs whenever payment is attempted on an order, and the payment fails.
        public const string OrderPaymentFailed = "order.payment_failed";
        //
        // Summary:
        //     Occurs whenever an order is created.
        public const string OrderCreated = "order.created";
        //
        // Summary:
        //     Occurs whenever an issuing transaction is updated.
        public const string IssuingTransactionUpdated = "issuing_transaction.updated";
        //
        // Summary:
        //     Occurs whenever an issuing transaction is created.
        public const string IssuingTransactionCreated = "issuing_transaction.created";
        //
        // Summary:
        //     Occurs whenever Stripe attempts to send a payout and that transfer fails.
        public const string PayoutFailed = "payout.failed";
        //
        // Summary:
        //     Occurs whenever an issuing dispute is updated.
        public const string IssuingDisputeUpdated = "issuing_dispute.updated";
        //
        // Summary:
        //     Occurs whenever an issuing cardholder is updated.
        public const string IssuingCardholderUpdated = "issuing_cardholder.updated";
        //
        // Summary:
        //     Occurs whenever an issuing cardholder is created.
        public const string IssuingCardholderCreated = "issuing_cardholder.created";
        //
        // Summary:
        //     Occurs whenever an issuing card is updated.
        public const string IssuingCardUpdated = "issuing_card.updated";
        //
        // Summary:
        //     Occurs whenever an issuing card is created.
        public const string IssuingCardCreated = "issuing_card.created";
        //
        // Summary:
        //     Occurs whenever an issuing authorization is updated.
        public const string IssuingAuthorizationUpdated = "issuing_authorization.updated";
        //
        // Summary:
        //     Occurs whenever an issuing authorization request is sent.
        public const string IssuingAuthorizationRequest = "issuing_authorization.request";
        //
        // Summary:
        //     Occurs whenever an issuing authorization is created.
        public const string IssuingAuthorizationCreated = "issuing_authorization.created";
        //
        // Summary:
        //     Occurs whenever an invoice item is updated.
        public const string InvoiceItemUpdated = "invoiceitem.updated";
        //
        // Summary:
        //     Occurs whenever an invoice item is deleted.
        public const string InvoiceItemDeleted = "invoiceitem.deleted";
        //
        // Summary:
        //     Occurs whenever an issuing dispute is created.
        public const string IssuingDisputeCreated = "issuing_dispute.created";
        //
        // Summary:
        //     Occurs whenever an invoice item is created.
        public const string InvoiceItemCreated = "invoiceitem.created";
        //
        // Summary:
        //     Occurs whenever a payout is expected to be available in the destination bank
        //     account. If the payout failed, a payout.failed webhook will additionally be sent
        //     at a later time.
        public const string PayoutPaid = "payout.paid";
        //
        // Summary:
        //     Occurs whenever a plan is created.
        public const string PlanCreated = "plan.created";
        //
        // Summary:
        //     Occurs whenever a transfer is reversed, including partial reversals.
        public const string TransferReversed = "transfer.reversed";
        //
        // Summary:
        //     Occurs whenever a new transfer is created.
        public const string TransferCreated = "transfer.created";
        //
        // Summary:
        //     Occurs whenever a source transaction is updated.
        public const string SourceTransactionUpdated = "source.transaction.updated";
        //
        // Summary:
        //     Occurs whenever a source transaction is created.
        public const string SourceTransactionCreated = "source.transaction.created";
        //
        // Summary:
        //     Occurs whenever a source is failed.
        public const string SourceFailed = "source.failed";
        //
        // Summary:
        //     Occurs whenever a source transitions to chargeable.
        public const string SourceChargeable = "source.chargeable";
        //
        // Summary:
        //     Occurs whenever a source is canceled.
        public const string SourceCanceled = "source.canceled";
        //
        // Summary:
        //     Occurs whenever a SKU is updated.
        public const string SkuUpdated = "sku.updated";
        //
        // Summary:
        //     Occurs whenever a SKU is deleted.
        public const string SkuDeleted = "sku.deleted";
        //
        // Summary:
        //     Occurs whenever the metadata of a payout is updated.
        public const string PayoutUpdated = "payout.updated";
        //
        // Summary:
        //     Occurs whenever a SKU is created.
        public const string SkuCreated = "sku.created";
        //
        // Summary:
        //     Occurs whenever a review is closed. The review's reason field will indicate why
        //     the review was closed (e.g. approved, refunded, refunded_as_fraud, disputed.
        public const string ReviewClosed = "review.closed";
        //
        // Summary:
        //     Occurs whenever a recipient is updated.
        public const string RecipientUpdated = "recipient.updated";
        //
        // Summary:
        //     Occurs whenever a recipient is deleted.
        public const string RecipientDeleted = "recipient.deleted";
        //
        // Summary:
        //     Occurs whenever a recipient is created.
        public const string RecipientCreated = "recipient.created";
        //
        // Summary:
        //     Occurs whenever a product is updated.
        public const string ProductUpdated = "product.updated";
        //
        // Summary:
        //     Occurs whenever a product is deleted.
        public const string ProductDeleted = "product.deleted";
        //
        // Summary:
        //     Occurs whenever a product is created.
        public const string ProductCreated = "product.created";
        //
        // Summary:
        //     Occurs whenever a plan is updated.
        public const string PlanUpdated = "plan.updated";
        //
        // Summary:
        //     Occurs whenever a plan is deleted.
        public const string PlanDeleted = "plan.deleted";
        //
        // Summary:
        //     Occurs whenever a review is opened.
        public const string ReviewOpened = "review.opened";
        //
        // Summary:
        //     Occurs whenever an invoice changes (for example, the amount could change).
        public const string InvoiceUpdated = "invoice.updated";
        //
        // Summary:
        //     Occurs X number of days before a subscription is scheduled to create an invoice
        //     that is charged automatically, where X is determined by your subscriptions settings.
        public const string InvoiceUpcoming = "invoice.upcoming";
        //
        // Summary:
        //     Occurs whenever an invoice email is sent out.
        public const string InvoiceSent = "invoice.sent";
        //
        // Summary:
        //     Occurs whenever a charge description or metadata is updated.
        public const string ChargeUpdated = "charge.updated";
        //
        // Summary:
        //     Occurs whenever a new charge is created and is successful.
        public const string ChargeSucceeded = "charge.succeeded";
        //
        // Summary:
        //     Occurs whenever a charge is refunded, including partial refunds.
        public const string ChargeRefunded = "charge.refunded";
        //
        // Summary:
        //     Occurs whenever a refund is updated on selected payment methods.
        public const string ChargeRefundUpdated = "charge.refund.updated";
        //
        // Summary:
        //     Occurs whenever a pending charge is created.
        public const string ChargePending = "charge.pending";
        //
        // Summary:
        //     Occurs whenever a failed charge attempt occurs.
        public const string ChargeFailed = "charge.failed";
        //
        // Summary:
        //     Occurs whenever a previously uncaptured charge expires.
        public const string ChargeExpired = "charge.expired";
        //
        // Summary:
        //     Occurs whenever a previously uncaptured charge is captured.
        public const string ChargeCaptured = "charge.captured";
        //
        // Summary:
        //     Occurs whenever bitcoin is pushed to a receiver.
        public const string BitcoinReceiverTransactionUpdated = "bitcoin.receiver.transaction.created";
        //
        // Summary:
        //     Occurs when the dispute is closed and the dispute status changes to charge_refunded,
        //     lost, warning_closed, or won.
        public const string ChargeDisputeClosed = "charge.dispute.closed";
        //
        // Summary:
        //     Occurs whenever a receiver is updated.
        public const string BitcoinReceiverUpdated = "bitcoin.receiver.updated";
        //
        // Summary:
        //     Occurs whenever a receiver has been created.
        public const string BitcoinReceiverCreated = "bitcoin.receiver.created";
        //
        // Summary:
        //     Occurs whenever your Stripe balance has been updated (e.g. when a charge collected
        //     is available to be paid out). By default, Stripe will automatically transfer
        //     any funds in your balance to your bank account on a daily basis.
        public const string BalanceAvailable = "balance.available";
        //
        // Summary:
        //     Occurs whenever an application fee refund is updated.
        public const string ApplicationFeeRefundUpdated = "application_fee.refund.updated";
        //
        // Summary:
        //     Occurs whenever an application fee is refunded, whether from refunding a charge
        //     or from refunding the application fee directly, including partial refunds.
        public const string ApplicationFeeRefunded = "application_fee.refunded";
        //
        // Summary:
        //     Occurs whenever an application fee is created on a charge.
        public const string ApplicationFeeCreated = "application_fee.created";
        //
        // Summary:
        //     Occurs whenever an external account is updated.
        public const string AccountExternalAccountUpdated = "account.external_account.updated";
        //
        // Summary:
        //     Occurs whenever an external account is deleted.
        public const string AccountExternalAccountDeleted = "account.external_account.deleted";
        //
        // Summary:
        //     Occurs whenever an external account is created.
        public const string AccountExternalAccountCreated = "account.external_account.created";
        //
        // Summary:
        //     Occurs whenever a user deauthorizes an application. Sent to the related application
        //     only.
        public const string AccountApplicationDeauthorized = "account.application.deauthorized";
        //
        // Summary:
        //     Occurs whenever a receiver is filled (that is, when it has received enough bitcoin
        //     to process a payment of the same amount).
        public const string BitcoinReceiverFilled = "bitcoin.receiver.filled";
        //
        // Summary:
        //     Occurs whenever a customer disputes a charge with their bank (chargeback).
        public const string ChargeDisputeCreated = "charge.dispute.created";
        //
        // Summary:
        //     Occurs when funds are reinstated to your account after a dispute is won.
        public const string ChargeDisputeFundsReinstated = "charge.dispute.funds_reinstated";
        //
        // Summary:
        //     Occurs when funds are removed from your account due to a dispute.
        public const string ChargeDisputeFundsWithdrawn = "charge.dispute.funds_withdrawn";
        //
        // Summary:
        //     Occurs whenever an invoice attempts to be paid, and the payment succeeds.
        public const string InvoicePaymentSucceeded = "invoice.payment_succeeded";
        //
        // Summary:
        //     Occurs whenever an invoice attempts to be paid, and the payment fails. This can
        //     occur either due to a declined payment, or because the customer has no active
        //     card. A particular case of note is that if a customer with no active card reaches
        //     the end of its free trial, an invoice.payment_failed notification will occur.
        public const string InvoicePaymentFailed = "invoice.payment_failed";
        //
        // Summary:
        //     Occurs whenever a new invoice is created. If you are using webhooks, Stripe will
        //     wait one hour after they have all succeeded to attempt to pay the invoice; the
        //     only exception here is on the first invoice, which gets created and paid immediately
        //     when you subscribe a customer to a plan. If your webhooks do not all respond
        //     successfully, Stripe will continue retrying the webhooks every hour and will
        //     not attempt to pay the invoice. After 3 days, Stripe will attempt to pay the
        //     invoice regardless of whether or not your webhooks have succeeded.
        public const string InvoiceCreated = "invoice.created";
        //
        // Summary:
        //     Occurs three days before the trial period of a subscription is scheduled to end.
        public const string CustomerSubscriptionUpdated = "customer.subscription.updated";
        //
        // Summary:
        //     Occurs three days before the trial period of a subscription is scheduled to end.
        public const string CustomerSubscriptionTrialWillEnd = "customer.subscription.trial_will_end";
        //
        // Summary:
        //     Occurs whenever a customer ends their subscription.
        public const string CustomerSubscriptionDeleted = "customer.subscription.deleted";
        //
        // Summary:
        //     Occurs whenever a customer with no subscription is signed up for a plan.
        public const string CustomerSubscriptionCreated = "customer.subscription.created";
        //
        // Summary:
        //     Occurs whenever a source's details are changed.
        public const string CustomerSourceUpdated = "customer.source.updated";
        //
        // Summary:
        //     Occurs whenever a source will expire at the end of the month.
        public const string CustomerSourceExpiring = "customer.source.expiring";
        //
        // Summary:
        //     Occurs whenever a source is removed from a customer.
        public const string CustomerSourceDeleted = "customer.source.deleted";
        //
        // Summary:
        //     Occurs whenever a new source is created for the customer.
        public const string CustomerSourceCreated = "customer.source.created";
        //
        // Summary:
        //     Occurs whenever a customer is switched from one coupon to another.
        public const string CustomerDiscountUpdated = "customer.discount.updated";
        //
        // Summary:
        //     Occurs whenever a customer's discount is removed.
        public const string CustomerDiscountDeleted = "customer.discount.deleted";
        //
        // Summary:
        //     Occurs whenever a coupon is attached to a customer.
        public const string CustomerDiscountCreated = "customer.discount.created";
        //
        // Summary:
        //     Occurs whenever any property of a customer changes.
        public const string CustomerUpdated = "customer.updated";
        //
        // Summary:
        //     Occurs whenever a customer is deleted.
        public const string CustomerDeleted = "customer.deleted";
        //
        // Summary:
        //     Occurs whenever a new customer is created.
        public const string CustomerCreated = "customer.created";
        //
        // Summary:
        //     Occurs whenever a coupon is updated.
        public const string CouponUpdated = "coupon.updated";
        //
        // Summary:
        //     Occurs whenever a coupon is deleted.
        public const string CouponDeleted = "coupon.deleted";
        //
        // Summary:
        //     Occurs whenever a coupon is created.
        public const string CouponCreated = "coupon.created";
        //
        // Summary:
        //     Occurs when the dispute is updated (usually with evidence).
        public const string ChargeDisputeUpdated = "charge.dispute.updated";
        //
        // Summary:
        //     Occurs whenever the description or metadata of a transfer is updated.
        public const string TransferUpdated = "transfer.updated";
        //
        // Summary:
        //     May be sent by Stripe at any time to see if a provided webhook URL is working.
        public const string Ping = "ping";
    }
}



#region @@Reference
/*
https://medium.com/c-sharp/stripe-webhooks-handling-in-asp-net-mvc-c-cff9889152b9

 
*/
#endregion
