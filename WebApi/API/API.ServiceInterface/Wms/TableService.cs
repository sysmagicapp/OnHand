﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.ServiceModel;
using WebApi.ServiceModel.Wms;

namespace WebApi.ServiceInterface.Wms
{
    public class TableService
    {
        public void TS_Rcbp(Auth auth, Rcbp request, Rcbp_Logic rcbp_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                
                    if (uri.IndexOf("wms/rcbp1/All") > 0)
                {
                    ecr.data.results = rcbp_Logic.Get_Rcbp1_All(request);
                }
                else if (uri.IndexOf("/wms/rcbp1") > 0)
                {
                    ecr.data.results = rcbp_Logic.Get_Rcbp1_List(request);
                }
                else if (uri.IndexOf("/wms/rcdg1/UnNo") > 0)
                {
                    ecr.data.results = rcbp_Logic.Get_Rcdg1UnNo_List(request);
                }
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }
        public void TS_Imgr(Auth auth, Imgr request, Imgr_Logic imgr_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (uri.IndexOf("/wms/imgr1/confirm") > 0)
                {
                    ecr.data.results = imgr_Logic.Confirm_Imgr1(request);
                }
                else if (uri.IndexOf("/wms/imgr1") > 0)
                {
                    ecr.data.results = imgr_Logic.Get_Imgr1_List(request);
                }
                else if (uri.IndexOf("/wms/imgr2/putaway/update") > 0)
                {
                    ecr.data.results = imgr_Logic.Update_Imgr2_StoreNo(request);
                }
                else if (uri.IndexOf("/wms/imgr2/qtyremark") > 0)
                {
                    ecr.data.results = imgr_Logic.Update_Imgr2_QtyRemark(request);
                }

                else if (uri.IndexOf("/wms/imgr2/putaway") > 0)
                {
                    ecr.data.results = imgr_Logic.Get_Imgr2_Putaway_List(request);
                }
                else if (uri.IndexOf("/wms/imgr2/transfer") > 0)
                {
                    ecr.data.results = imgr_Logic.Get_Imgr2_Transfer_List(request);
                }
                else if (uri.IndexOf("/wms/imgr2/receipt") > 0)
                {
                    ecr.data.results = imgr_Logic.Get_Imgr2_Receipt_List(request);
                }
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }
        public void TS_Impr(Auth auth, Impr request, Impr_Logic impr_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (uri.IndexOf("/wms/impr1") > 0)
                {
                    ecr.data.results = impr_Logic.Get_Impr1(request);
                }
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }
        public void TS_Whwh(Auth auth, Whwh request, Whwh_Logic whwh_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (uri.IndexOf("/wms/whwh1") > 0)
                {
                    ecr.data.results = whwh_Logic.Get_Whwh1_List(request);
                }
                else if (uri.IndexOf("/wms/whwh2") > 0)
                {
                    ecr.data.results = whwh_Logic.Get_Whwh2_List(request);
                }
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }
        public void TS_Saal(Auth auth, Saal request, Saal_Logic saal_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (uri.IndexOf("/wms/saal/create") > 0)
                {
                    ecr.data.results = saal_Logic.Update_Saal(request);
                }
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }

        public void TS_Imgi(Auth auth, Imgi request, Imgi_Logic imgi_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (uri.IndexOf("/wms/imgi1/update") > 0)
                {
                    ecr.data.results = imgi_Logic.Update_Imgi1_Status(request);
                }
                else if (uri.IndexOf("/wms/imgi1") > 0)
                {
                    ecr.data.results = imgi_Logic.Get_Imgi1_List(request);
                }
                else if (uri.IndexOf("/wms/imgi2/picking") > 0)
                {
                    ecr.data.results = imgi_Logic.Get_Imgi2_Picking_List(request);
                }
                else if (uri.IndexOf("/wms/imgi2/verify") > 0)
                {
                    ecr.data.results = imgi_Logic.Get_Imgi2_Verify_List(request);
                }
                else if (uri.IndexOf("/wms/imgi2/qtyremark") > 0)
                {
                    ecr.data.results = imgi_Logic.Update_Imgi2_QtyRemark(request);
                }
                else if (uri.IndexOf("/wms/imgi2/packingno") > 0)
                {
                    ecr.data.results = imgi_Logic.Update_Imgi2_PackingNo(request);
                }
                else if (uri.IndexOf("/wms/imgi3/picking/confim") > 0)
                {
                    ecr.data.results = imgi_Logic.Comfirm_Picking_Imgi3(request);
                }
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }

        public void TS_Imsn(Auth auth, Imsn request, Imsn_Logic imsn_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (uri.IndexOf("/wms/imsn1/create") > 0)
                {
                    ecr.data.results = imsn_Logic.Insert_Imsn1(request);
                }
                else if (uri.IndexOf("/wms/imsn1") > 0)
                {
                    ecr.data.results = imsn_Logic.Get_Imsn1_List(request);
                }
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }
        public void TS_Imit(Auth auth, Imit request, Imit_Logic imit_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (uri.IndexOf("/wms/imit1/create") > 0)
                {
                    ecr.data.results = imit_Logic.Insert_Imit1(request);
                }
                else if (uri.IndexOf("/wms/imit1/confirm") > 0)
                {
                    ecr.data.results = imit_Logic.Confirm_Imit1(request);
                }
                else if (uri.IndexOf("/wms/imit2/create") > 0)
                {
                    ecr.data.results = imit_Logic.Insert_Imit2(request);
                }
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }
        public void TS_Impm(Auth auth, Impm request, Impm_Logic impm_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (uri.IndexOf("/wms/impm1/enquiry") > 0)
                {
                    ecr.data.results = impm_Logic.Get_Impm1_Enquiry_List(request);
                }
                else if (uri.IndexOf("/wms/impm1/transfer") > 0)
                {
                    ecr.data.results = impm_Logic.Get_Impm1_Transfer_List(request);
                }
                else if (uri.IndexOf("/wms/impm1") > 0)
                {
                    ecr.data.results = impm_Logic.Get_Impm1_List(request);
                }
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }
        public void TS_Rcpk(Auth auth, Rcpk request, rcpk_loigc rcpk_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {
                if (uri.IndexOf("/wms/Rcpk") > 0)
                {
                    ecr.data.results = rcpk_Logic.Get_Rcpk_List(request);
                }
                
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }

        public void TS_aeaw(Auth auth, aeaw request, Aeaw_Logic aeaw_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {

                if (uri.IndexOf("/wms/awaw1/MAwbNo") > 0)
                {
                    ecr.data.results = aeaw_Logic.Get_Aeaw1_List(request);
                }
                else if (uri.IndexOf("/wms/awaw1/MasterJobNo") > 0)
                {
                    ecr.data.results = aeaw_Logic.Get_Aeaw1MasterJoNo_List(request);
                }
                else if (uri.IndexOf("/wms/awaw1/Pid") > 0)
                {
                    ecr.data.results = aeaw_Logic.Get_PID_List(request);
                }
                else if (uri.IndexOf("/wms/Aemt1/Insert") > 0)
                {
                    //ecr.data.results = aeaw_Logic.Insert_Aemt1(request);
                }
                else if (uri.IndexOf("/wms/Aemt1/selectAll") > 0)
                {
                    ecr.data.results = aeaw_Logic.SelectAllAemt1(request);
                }

                else if (uri.IndexOf("/wms/Aemt1/getAemt") > 0)
                {
                    ecr.data.results = aeaw_Logic.getKeyAemt1(request);
                }
                else if (uri.IndexOf ("/wms/Aemt1/select")>0)
                {
                    ecr.data.results = aeaw_Logic.SelectAemt1(request);
                }
                else if (uri.IndexOf("/wms/Aemt1/Update") > 0)
                {
                    ecr.data.results = aeaw_Logic.Update_Aemt1(request);
                }
                else if (uri.IndexOf ("/wms/LOCATION_K/LOC_CODE") >0)
                {
                    ecr.data.results = aeaw_Logic.Get_LOC_CODE_List(request);
                }

                
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }
    

        public void TS_Imcc(Auth auth, ONHAND_D request, imcc_loigc imcc_Logic, CommonResponse ecr, string[] token, string uri)
        {
            if (auth.AuthResult(token, uri))
            {

                if (uri.IndexOf("/wms/ONHAND_D/confirm") > 0)
                {
                    ecr.data.results = imcc_Logic.ConfirmAll_ONHAND_D(request);
                }
                else if (uri.IndexOf("/wms/ONHAND_D/update") > 0)
                {
                    ecr.data.results = imcc_Logic.Update_ONHAND_D(request);
                }
                else if (uri.IndexOf("/wms/OH_PID_D/create") > 0)
                {
                    ecr.data.results = imcc_Logic.Create_OH_PID_D(request);

                }
                else if (uri.IndexOf("/wms/OH_PID_D/CheckAlreadyPid") > 0)
                {
                    ecr.data.results = imcc_Logic.CheckAlreadyPID(request);

                }
                else if (uri.IndexOf("/wms/OH_PID_D/UpdateUnNo") > 0)
                {
                    ecr.data.results = imcc_Logic.UpdateUnNo(request);

                }
                else if (uri.IndexOf("/wms/OH_PID_D/UpdatePidUnNo") > 0)
                {
                    ecr.data.results = imcc_Logic.UpdatePidUnNo(request);

                }
                else if (uri.IndexOf("/wms/OH_PID_D/DeleteLineItem") > 0)
                {
                    ecr.data.results = imcc_Logic.DeleteLineItem(request);

                }
                else if (uri.IndexOf("/wms/OH_PID_D/updateLineItem") > 0)
                {
                    ecr.data.results = imcc_Logic.UpdateLineItem(request);

                }
                
                else if (uri.IndexOf("/wms/OH_PID_D/TruckerBillNo") > 0)
                {
                    ecr.data.results = imcc_Logic.ValidateTRK_BILL_NO(request);
                }
                else if (uri.IndexOf("/wms/OH_PID_D/verifyTruckerBillNoBarCode") > 0)
                {
                    ecr.data.results = imcc_Logic.verifyTruckerBillNoBarCode(request);
                }
                else if (uri.IndexOf("/wms/OH_PID_D/validate") > 0)
                {
                    ecr.data.results = imcc_Logic.ValidatePID_NO(request);
                }
                else if (uri.IndexOf("wms/ONHAND_D/OnhandNo") > 0)
                {
                    ecr.data.results = imcc_Logic.Get_ONHANDNo(request);
                }
                else if (uri.IndexOf("/wms/ONHAND_D") > 0)
                {
                    ecr.data.results = imcc_Logic.Get_ONHAND_D_List(request);

                }
                else if (uri.IndexOf("/wms/OH_PID_D") > 0)
                {
                    ecr.data.results = imcc_Logic.Get_OH_PID_D_List(request);

                }

                else if (uri.IndexOf("/wms/ONHAND_D") > 0)
                {
                    ecr.data.results = imcc_Logic.Get_ONHAND_D_List(request);
                } 
                
                ecr.meta.code = 200;
                ecr.meta.message = "OK";
            }
            else
            {
                ecr.meta.code = 401;
                ecr.meta.message = "Unauthorized";
            }
        }
    }
}
