using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    public class Selling : lib.DomainBaseStamp
    {
        public Selling(int id, long stamp, DateTime? updated, string updater, lib.DomainObjectState mstate
            ) :base(id, stamp, updated, updater, mstate)
        {

        }

        private Parcel myparcel;
        public Parcel Parcel
        { set { SetProperty<Parcel>(ref myparcel, value); } get { return myparcel; } }

        public decimal? InvoiceCB2
        {
            get
            {
                return myparcel.Requests.Where(
                    (Request request) => {return request.ParcelId.HasValue; }
                                                ).Sum(
                                                      (Request prequest) => {
                                                            return prequest.CustomerLegals.Sum(
                                                                (RequestCustomerLegal customer) => {
                                                                    return customer.Prepays.Sum(
                                                                        (PrepayCustomerRequest requestprepay) => { return requestprepay.DTSum * requestprepay.Prepay.CBRatep2p; }
                                                                    );
                                                                }
                                                            );
                                                       }
                                                    );
            }
        }
        public decimal? InvoiceDT
        {
            get
            {
                return myparcel.Specifications.Sum(
                                                      (Specification.Specification spec) => {
                                                          return spec.Details.Sum(
                                                                (Specification.SpecificationDetail detail) => { return detail.Cost; }
                                                          );
                                                      }
                                                    );
            }
        }


        protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
        {
        }
        protected override void RejectProperty(string property, object value)
        {
        }
    }
}
