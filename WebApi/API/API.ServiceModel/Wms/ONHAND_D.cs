﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServiceStack;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebApi.ServiceModel.Wms
{
    [Route("/wms/ONHAND_D", "Get")]   //ONHAND_D?CustomerCode ,ONHAND_D?TrxNo
    [Route("/wms/ONHAND_D", "Get")]     //ONHAND_D?TrxNo
    [Route("/wms/ONHAND_D/OnhandNo", "Get")]     //ONHAND_D?TrxNo
    [Route("/wms/OH_PID_D", "Get")]     //OH_PID_D?OnhandNO
    [Route("/wms/OH_PID_D/create", "Post")]     //OH_PID_D?OnhandNO
    [Route("/wms/OH_PID_D/DeleteLineItem", "Get")]     //OH_PID_D?OnhandNO,
    [Route("/wms/ONHAND_D/confirm", "Post")]
    [Route("/wms/ONHAND_D/update", "Post")]
    [Route("/wms/OH_PID_D/updateLineItem", "Post")]
    [Route("/wms/OH_PID_D/UpdateUnNo", "get")]
    [Route("/wms/OH_PID_D/validate", "Get")]     //OH_PID_D?PID_NO
    [Route("/wms/OH_PID_D/TruckerBillNo", "Get")]     //OH_PID_D?PID_NO
    [Route("/wms/OH_PID_D/verifyTruckerBillNoBarCode", "Get")]     //OH_PID_D?PID_NO
    [Route("/wms/OH_PID_D/UpdatePidUnNo", "Get")]     //OH_PID_D?PID_NO
    [Route("/wms/OH_PID_D/CheckAlreadyPid", "Get")]     //OH_PID_D?PID_NO
    public class ONHAND_D : IReturn<CommonResponse>
    {
        public string strONHAND_NO { get; set; }
        public string CustomerCode { get; set; }
        public string TrxNo { get; set; }
        public string UpdateAllString { get; set; }
        public string NextNo { get; set; }
        public string LineItemNo { get; set; }
        public string strPID_NO { get; set; }
        public string strTRK_BILL_NO { get; set; }
        public string UnNoIndex { get; set; }
        public string UnNo { get; set; }
        public string TruckerBillNo { get; set; }
    }
    public class imcc_loigc
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public List<ONHAND_D_Table> Get_ONHAND_D_List(ONHAND_D request)
        {
            List<ONHAND_D_Table> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
                {

                    if (!string.IsNullOrEmpty(request.strONHAND_NO))
                    {

                        string strSQL = "";
                            strSQL=" select  "  +
                            " ISNULL(SHP_CODE, '') AS SHP_CODE, " +
                            " ISNULL((select BusinessPartyName from rcbp1 where BusinessPartyCode = SHP_CODE ),'')  AS  'ShipperName', " +
                            " ISNULL(shipperaddress1,'') AS shipperaddress1, " +
                            " ISNULL(ShipperAddress2,'') AS ShipperAddress2, " +
                            " ISNULL(ShipperAddress3,'') AS ShipperAddress3, " +
                            " ISNULL(ShipperAddress4,'') AS ShipperAddress4, " +
                            " ISNULL(CNG_CODE,'') AS CNG_CODE, " +
                            " ISNULL( (select  BusinessPartyName from rcbp1 where BusinessPartyCode = CNG_CODE),'' ) AS  'ConsigneeName', " +
                            " ISNULL(ConsigneeAddress1,'') AS ConsigneeAddress1, " +
                            " ISNULL(ConsigneeAddress2,'') AS ConsigneeAddress2, " +
                            " ISNULL(ConsigneeAddress3,'') AS ConsigneeAddress3, " +
                            " ISNULL(ConsigneeAddress4,'') AS ConsigneeAddress4, " +
                            " ISNULL(SHP_MODE,'') AS SHP_MODE, " +
                            " ONHAND_date, " +
                            " ISNULL(CASE_NO,'') AS CASE_NO, " +
                            " ISNULL( PUB_YN,'') AS PUB_YN, " +
                            " ISNULL(HAZARDOUS_YN,'') AS HAZARDOUS_YN , " +
                            " ISNULL(CLSF_YN,'') AS CLSF_YN, " +
                            " ISNULL(ExerciseFlag,'') AS ExerciseFlag , " +
                            " ISNULL(LOC_CODE,'') AS LOC_CODE, " +
                            " ISNULL(TRK_CODE,'') AS TRK_CODE, " +
                            " ISNULL(TRK_CHRG_TYPE,'') AS TRK_CHRG_TYPE, " +
                            " ISNULL(PICKUP_SUP_datetime,'') AS PICKUP_SUP_datetime, " +
                            " ISNULL(NO_INV_WH,0) AS  NO_INV_WH, " +              
                            " ISNULL((select  sum(PIECES) from OH_PID_D where onhand_no = ONHAND_D.onhand_no ), 0) AS TotalPCS,   " +
                            " ISNULL((select  sum(GROSS_LB) from OH_PID_D where onhand_no = ONHAND_D.onhand_no ),0 ) AS TotalWeight   " +                  
                            " from ONHAND_D  where onhand_no ='" + request.strONHAND_NO + "'";
                        Result = db.Select<ONHAND_D_Table>(strSQL);
                 
                    }
                }
                              
            }
            catch { throw; }
            return Result;
        }

        public List<ONHAND_D_Table> Get_ONHANDNo(ONHAND_D request)
        {

           List<ONHAND_D_Table> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
                {

                    if (!string.IsNullOrEmpty(request.strONHAND_NO))
                    {

                        string strSQL = "Select onhand_no  From ONHAND_D Where  (  OH_STAT='RI' or  OH_STAT='OP'  or  OH_STAT='DT' )    And onhand_no LIKE '" + request.strONHAND_NO + "%'  Order By onhand_no Asc";
                        Result = db.Select<ONHAND_D_Table>(strSQL);                 
                    }
                }

            }
            catch { throw; }
            return Result;
        }
        public List<ON_PID_D> Get_OH_PID_D_List(ONHAND_D request)
        {
            List<ON_PID_D> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
                {

                    if (!string.IsNullOrEmpty(request.strONHAND_NO))
                    {

                        string strSQL = "";
                        strSQL = " select  " +
                        " RowNum = ROW_NUMBER() OVER(ORDER BY OH_PID_D.LineItemNo ASC) , " +
                        " OH_PID_D.ONHAND_NO  , " +
                        " OH_PID_D.LineItemNo , " +
                        " ISNULL(OH_PID_D.PACK_TYPE,'') AS PACK_TYPE, " +
                        " ISNULL(OH_PID_D.TRK_BILL_NO,'') AS TRK_BILL_NO , " +
                        " ISNULL(OH_PID_D.PID_NO,'') AS PID_NO , " +
                        " ISNULL(OH_PID_D.UnNo,'') AS UnNo , " +
                        " OH_PID_D.LENGTH, " +
                        " OH_PID_D.WIDTH, " +
                        " OH_PID_D.HEIGHT, " +
                        " OH_PID_D.GROSS_LB, " +
                        " OH_PID_D.PIECES, " +                     
                        " ISNULL(ONHAND_D.SHP_CODE, '') AS SHP_CODE, " +
                        " ISNULL((select BusinessPartyName from rcbp1 where BusinessPartyCode = ONHAND_D.SHP_CODE ),'')  AS  'ShipperName', " +
                        " ISNULL(ONHAND_D.CNG_CODE,'') AS CNG_CODE, " +
                        " ISNULL( (select  BusinessPartyName from rcbp1 where BusinessPartyCode = ONHAND_D.CNG_CODE),'' ) AS  'ConsigneeName', " +
                        " ONHAND_D.ONHAND_date, " +
                        " ISNULL(ONHAND_D.CASE_NO,'') AS CASE_NO, " +
                        " ISNULL( ONHAND_D.PUB_YN,'') AS PUB_YN, " +
                        " ISNULL(ONHAND_D.HAZARDOUS_YN,'') AS HAZARDOUS_YN , " +
                        " ISNULL(ONHAND_D.CLSF_YN,'') AS CLSF_YN, " +
                        " ISNULL(ONHAND_D.ExerciseFlag,'') AS ExerciseFlag , " +
                        " ISNULL(ONHAND_D.LOC_CODE,'') AS LOC_CODE, " +
                        " ISNULL(ONHAND_D.TRK_CODE,'') AS TRK_CODE, " +
                        " ISNULL(ONHAND_D.TRK_CHRG_TYPE,'') AS TRK_CHRG_TYPE, " +
                        " ONHAND_D.PICKUP_SUP_datetime, " +
                        " ISNULL(ONHAND_D.NO_INV_WH,0) AS  NO_INV_WH, " +
                        " ISNULL((select  sum(PIECES) from OH_PID_D  ), 0) AS TotalPCS,   " +
                        " ISNULL((select  sum(GROSS_LB) from OH_PID_D  ),0 ) AS TotalWeight,    " +
                        " ISNULL (UnNo01,'') as UnNo01 , "+
                        " ISNULL ( UnNo02,'') as UnNo02 , " +
                        " ISNULL ( UnNo03,'') as UnNo03 , " +
                        " ISNULL ( UnNo04,'') as UnNo04 , " +
                        " ISNULL ( UnNo05 ,'') as UnNo05 , " +
                        " ISNULL ( UnNo06 ,'') as UnNo06 , " +
                        " ISNULL ( UnNo07 ,'') as UnNo07 , " +
                        " ISNULL ( UnNo08 ,'') as UnNo08 , " +
                        " ISNULL ( UnNo09 ,'') as UnNo09 , " +
                        " ISNULL ( UnNo10 ,'') as UnNo10 , " +
                        " ISNULL( (Select DgClass From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo01) ,'') AS  DgClass01 ," +
                        " ISNULL( (Select DgClass From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo02) ,'') AS  DgClass02 ," +
                        " ISNULL( (Select DgClass From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo03) ,'') AS  DgClass03 ," +
                        " ISNULL( (Select DgClass From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo04) ,'') AS  DgClass04 ," +
                        " ISNULL( (Select DgClass From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo05) ,'') AS  DgClass05 ," +
                        " ISNULL( (Select DgClass From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo06) ,'') AS  DgClass06 ," +
                        " ISNULL( (Select DgClass From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo07) ,'') AS  DgClass07 ," +
                        " ISNULL( (Select DgClass From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo08) ,'') AS  DgClass08 ," +
                        " ISNULL( (Select DgClass From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo09) ,'') AS  DgClass09 ," +
                        " ISNULL( (Select DgClass From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo10) ,'') AS  DgClass10 ," +
                        " ISNULL( (Select DgDescription From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo01) ,'') AS  DgDescription01 ," +
                        " ISNULL( (Select DgDescription From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo02) ,'') AS  DgDescription02 ," +
                        " ISNULL( (Select DgDescription From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo03) ,'') AS  DgDescription03 ," +
                        " ISNULL( (Select DgDescription From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo04) ,'') AS  DgDescription04 ," +
                        " ISNULL( (Select DgDescription From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo05) ,'') AS  DgDescription05 ," +
                        " ISNULL( (Select DgDescription From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo06) ,'') AS  DgDescription06 ," +
                        " ISNULL( (Select DgDescription From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo07) ,'') AS  DgDescription07 ," +
                        " ISNULL( (Select DgDescription From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo08) ,'') AS  DgDescription08 ," +
                        " ISNULL( (Select DgDescription From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo09) ,'') AS  DgDescription09 ," +
                        " ISNULL( (Select DgDescription From Rcdg1 Where Rcdg1.UnNo=OH_PID_D.UnNo10) ,'') AS  DgDescription10 ," +
                        " ISNULL(OH_PID_D.Remark ,'') AS Remark    " +
                        " from OH_PID_D left join ONHAND_D on ONHAND_D.onhand_no=OH_PID_D.onhand_no  where OH_PID_D.onhand_no='" + request.strONHAND_NO + "'";
                        //   " from OH_PID_D left join ONHAND_D on ONHAND_D.onhand_no=OH_PID_D.onhand_no  where OH_PID_D.onhand_no='ONHAND06'  ";
                        Result = db.Select<ON_PID_D>(strSQL);

                    }
                }

            }
            catch { throw; }
            return Result;
        }



        public List<ON_PID_D> verifyTruckerBillNoBarCode(ONHAND_D request)
        {

            List<ON_PID_D> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
                {

                    if (!string.IsNullOrEmpty(request.strONHAND_NO) && string.IsNullOrEmpty(request.TruckerBillNo) )
                    {

                        string strSQL = "";              
                        Result = db.Select<ON_PID_D>(strSQL);

                    }
                }

            }
            catch { throw; }
            return Result;
        }



        public int UpdateLineItem(ONHAND_D request) {

            int  Result =-1;
            string Result_SP_Ind_YN = "";
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (request.UpdateAllString != null && request.UpdateAllString != "")
                    {
                        JArray ja = (JArray)JsonConvert.DeserializeObject(request.UpdateAllString);
                        if (ja != null)
                        {

                            for (int i = 0; i < ja.Count(); i++)
                            {

                                string strSql = "";
                                string strSP_Ind_YN = "";
                                int LENGTH = 0;
                                int WIDTH = 0;
                                int HEIGHT = 0;
                                int GROSS_LB = 0;
                                int lineItemNo = int.Parse(ja[i]["LineItemNo"].ToString());
                                if (lineItemNo > 0)
                                {
                                    string OnhandNo= ja[i]["ONHAND_NO"].ToString();
                                    string strUserId = ja[i]["UserID"].ToString();
                                    string PACK_TYPE = ja[i]["PACK_TYPE"].ToString();
                                   
                                    Result_SP_Ind_YN = db.SqlScalar<string>(" select  dbo.Set_SP_Ind_YN (@TypeDesc)", new {  TypeDesc = PACK_TYPE });
                                    strSP_Ind_YN = Result_SP_Ind_YN;
                                    string PackTypeCode =getPackTypeCode(Modfunction.SQLSafeValue(ja[i]["PACK_TYPE"].ToString()));
                                    string TRK_BILL_NO = ja[i]["TRK_BILL_NO"].ToString();
                                    string PID_NO = ja[i]["PID_NO"].ToString();
                                    string UnNo = ja[i]["UnNo"].ToString();
                                    string Remark = Modfunction.CheckNull(ja[i]["Remark"]);
                                    LENGTH = Modfunction.ReturnZero(ja[i]["LENGTH"].ToString());
                                    WIDTH = Modfunction.ReturnZero(ja[i]["WIDTH"].ToString());
                                    HEIGHT = Modfunction.ReturnZero(ja[i]["HEIGHT"].ToString());
                                    GROSS_LB = Modfunction.ReturnZero(ja[i]["GROSS_LB"].ToString());
                                                                                   
                                    strSql = " UPDATE OH_PID_D Set " +
                                              "ISSUER_CHANGED='" + strUserId + "'," +
                                              "Date_CHANGED=getdate()," +
                                              "PACK_TYPE='" + PACK_TYPE + "'," +
                                               "SP_IND='" + strSP_Ind_YN + "'," +                           
                                              "PackTypeCode='" + PackTypeCode + "'," +
                                              "TRK_BILL_NO="  +Modfunction.SQLSafeValue(TRK_BILL_NO) + "," +
                                              "PID_NO="+Modfunction.SQLSafeValue(PID_NO) + "," +
                                              "UnNo='" + UnNo + "'," +
                                              "LENGTH=" + LENGTH + "," +
                                              "WIDTH=" + WIDTH + "," +
                                              "HEIGHT=" + HEIGHT + "," +
                                              "GROSS_LB=" + GROSS_LB + ","+
                                              "Remark=" +Modfunction.SQLSafeValue (Remark)  + " " +
                                              "WHERE ONHAND_NO ='" + OnhandNo + "' And LineItemNo="+ lineItemNo + "  " +
                                              "";                               
                                    db.ExecuteSql(strSql);
                                    aggregateMaster(OnhandNo);

                                }

                            }
                        }
                        Result = 1;
                    }
                }

            }
            catch { throw; }
            return Result;
        }

        public List<ON_PID_D> UpdatePidUnNo(ONHAND_D request)
        {
        
            List<ON_PID_D> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (request.UnNo != null && request.UnNo != "" &&  request.strONHAND_NO != null && request.strONHAND_NO != "" && request.LineItemNo != null && request.LineItemNo != "" )
                    {

                        string strSqlPid = "";
                        string strSql = "";
                        string strUpdateUnNo = "";
                        strSqlPid = "Select  UnNo01,UnNo02,UnNo03,UnNo04,UnNo05,UnNo06,UnNo07,UnNo08,UnNo09,UnNo10 From  OH_PID_D Where ONHAND_NO='"+request.strONHAND_NO+"' And LineItemNo ='"+request.LineItemNo+"'";
                        Result = db.Select<ON_PID_D>(strSqlPid);


                        if (Modfunction.CheckNull(Result[0].UnNo01).Length <=0)
                        {
                            strUpdateUnNo = "UnNo01";
                        }
                        else if (Modfunction.CheckNull(Result[0].UnNo02).Length <=0)
                        {
                            strUpdateUnNo = "UnNo02";
                        }

                        else if (Modfunction.CheckNull(Result[0].UnNo03).Length <=0)
                        {
                            strUpdateUnNo = "UnNo03";
                        }
                        else if (Modfunction.CheckNull(Result[0].UnNo04).Length <=0)
                        {
                            strUpdateUnNo = "UnNo04";
                        }
                        else if (Modfunction.CheckNull(Result[0].UnNo05).Length <=0)
                        {
                            strUpdateUnNo = "UnNo05";
                        }
                       else if (Modfunction.CheckNull(Result[0].UnNo06).Length <=0)
                        {
                            strUpdateUnNo = "UnNo06";
                        }
                        else if (Modfunction.CheckNull(Result[0].UnNo07).Length <=0)
                        {
                            strUpdateUnNo = "UnNo07";
                        }

                        else if (Modfunction.CheckNull(Result[0].UnNo08).Length <=0)
                        {
                            strUpdateUnNo = "UnNo08";
                        }
                        else if (Modfunction.CheckNull(Result[0].UnNo09).Length <= 0)
                        {
                            strUpdateUnNo = "UnNo09";
                        }

                        else 
                        {
                            strUpdateUnNo = "UnNo10";
                        }

                        strSql = "Update OH_PID_D set " + strUpdateUnNo + " ='"+request.UnNo+"'" +
                         " Where " +
                         " ONHAND_NO ='" + request.strONHAND_NO + "'" +
                         " And LineItemNo=" + request.LineItemNo + "";
                        db.ExecuteSql(strSql);

                    }

                }         
            }
            catch { throw; }
            return Result;

        }

        public int UpdateUnNo(ONHAND_D request)
        {

            int Result = -1;

            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (request.strONHAND_NO != null && request.strONHAND_NO != "" && request.LineItemNo != null && request.LineItemNo != "" && request.UnNoIndex != null && request.UnNoIndex != "")
                    {
                        string strSql = "";
                        string strUpdateUnNo = "";
                        if (int.Parse(request.UnNoIndex) == 0)
                        {
                            strUpdateUnNo = "UnNo01";
                        } else if (int.Parse(request.UnNoIndex) == 1)
                        {
                            strUpdateUnNo = "UnNo02";
                        }
                        else if (int.Parse(request.UnNoIndex) == 2)
                        {
                            strUpdateUnNo = "UnNo03";
                        }
                        else if (int.Parse(request.UnNoIndex) == 3)
                        {
                            strUpdateUnNo = "UnNo04";
                        }
                        else if (int.Parse(request.UnNoIndex) == 4)
                        {
                            strUpdateUnNo = "UnNo05";
                        }
                        else if (int.Parse(request.UnNoIndex) == 5)
                        {
                            strUpdateUnNo = "UnNo06";
                        }
                        else if (int.Parse(request.UnNoIndex) == 6)
                        {
                            strUpdateUnNo = "UnNo07";
                        }
                        else if (int.Parse(request.UnNoIndex) ==7)
                        {
                            strUpdateUnNo = "UnNo08";
                        }
                        else if (int.Parse(request.UnNoIndex) == 8)
                        {
                            strUpdateUnNo = "UnNo09";
                        }
                        else 
                        {
                            strUpdateUnNo = "UnNo10";
                        }
                        strSql = "Update OH_PID_D set "+ strUpdateUnNo + " =''"+
                           " Where " +
                           " ONHAND_NO ='" + request.strONHAND_NO + "'" +
                           " And LineItemNo=" + request.LineItemNo + "";
                        db.ExecuteSql(strSql);
                     
                    }

                }
                Result = 1;

            }
            catch { throw; }
            return Result;
        }
        public int DeleteLineItem(ONHAND_D request)
        {

            int Result = -1;

            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (request.strONHAND_NO != null && request.strONHAND_NO != "" && request.LineItemNo != null && request.LineItemNo != "") {
                        string strSql = "";
                        strSql = "Delete from OH_PID_D where " +
                          "ONHAND_NO ='"+request .strONHAND_NO +"'"+
                          "And LineItemNo="+request.LineItemNo + "" ;
                        db.ExecuteSql(strSql);
                        aggregateMaster(request.strONHAND_NO);
                    }
                 
                }
                Result = 1;

            }
            catch { throw; }
            return Result;
        }

        public int aggregateMaster(string onhandNo) {
            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (onhandNo != "") {
                  string strSql = "";
                  strSql=  " UPDATE Onhand_D Set " +
                 " TOT_PCS = (SELECT SUM(ISNULL(PIECES, 0)) FROM OH_PID_D WHERE ONHAND_NO = '" + onhandNo + "'), " +
                 " No_PCS_WH = (SELECT SUM(ISNULL(PIECES, 0)) FROM OH_PID_D WHERE ONHAND_NO = '" + onhandNo + "'), " +
                 " Tot_trk_pkg  = (SELECT SUM(ISNULL(PIECES, 0)) FROM OH_PID_D WHERE ONHAND_NO = '" + onhandNo + "'), " +
                 " TOT_GROSS_LB = (SELECT SUM(ISNULL(GROSS_LB, 0)) FROM OH_PID_D WHERE ONHAND_NO = '" + onhandNo + "'), " +
                "  Tot_wt_wh = (SELECT SUM(ISNULL(GROSS_LB, 0)) FROM OH_PID_D WHERE ONHAND_NO = '" + onhandNo + "'), " +
                 " TOT_VOL_FT = (SELECT SUM(ISNULL(GROSS_LB, 0)) FROM OH_PID_D WHERE ONHAND_NO = '" + onhandNo + "'), " +
                 " TOT_VOL_LB = (SELECT SUM(CONVERT(INT, PIECES * LENGTH * WIDTH * HEIGHT / 1728) + ROUND((((PIECES * LENGTH * WIDTH * HEIGHT) % 1728) / 1728 * 100) / 100, 1)) FROM OH_PID_D WHERE " +
                 " ONHAND_NO = '" + onhandNo + "') " +
                 " WHERE ONHAND_NO ='"+ onhandNo + "'  " +
                 "";
                        db.ExecuteSql(strSql);
                    }
              
                }
                Result = 1;

            }
            catch { throw; }
            return Result;
        }
        public int Create_OH_PID_D(ONHAND_D request)
        {

            int Result = -1;

            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (request.UpdateAllString != null && request.UpdateAllString != "")
                    {
                        JArray ja = (JArray)JsonConvert.DeserializeObject(request.UpdateAllString);
                        if (ja != null)
                        {

                            for (int i = 0; i < ja.Count(); i++)
                            {

                                string strSql = "";
                                string Result_SP_Ind_YN = "";
                                string strSP_Ind_YN = "";
                                int LENGTH = 0;
                                int WIDTH = 0;
                                int HEIGHT = 0;
                                int GROSS_LB = 0;
                                double IND_VOL = 0;
                                double VOL_LB = 0;
                                double IND_CHG_LB = 0;
                                string OnhandNo = ja[i]["ONHAND_NO"].ToString();
                                int intMaxLineItemNo = 1;
                                if (OnhandNo!="")
                                {
                                    List<ON_PID_D> list1 = db.Select<ON_PID_D>("Select Max(LineItemNo) LineItemNo from OH_PID_D Where onhand_no = " + Modfunction.SQLSafeValue(OnhandNo));
                                    if (list1 != null)
                                    {
                                        if (list1[0].LineItemNo > 0)
                                            intMaxLineItemNo = list1[0].LineItemNo + 1;
                                    }

                                    string PACK_TYPE = ja[i]["PACK_TYPE"].ToString();
                                    string TRK_BILL_NO = ja[i]["TRK_BILL_NO"].ToString();                          
                                    string strUserId = ja[i]["UserID"].ToString();
                                    string PID_NO = ja[i]["PID_NO"].ToString();
                                    Result_SP_Ind_YN = db.SqlScalar<string>(" select  dbo.Set_SP_Ind_YN (@TypeDesc)", new { TypeDesc = PACK_TYPE });
                                    strSP_Ind_YN = Result_SP_Ind_YN;
                                    string UnNo = ja[i]["UnNo"].ToString();
                                    string UnNo01 = Modfunction.CheckNull(ja[i]["UnNo01"]);
                                    string UnNo02 = Modfunction.CheckNull(ja[i]["UnNo02"]);
                                    string UnNo03 = Modfunction.CheckNull(ja[i]["UnNo03"]);
                                    string UnNo04 = Modfunction.CheckNull(ja[i]["UnNo04"]);
                                    string UnNo05 = Modfunction.CheckNull(ja[i]["UnNo05"]);
                                    string UnNo06 = Modfunction.CheckNull(ja[i]["UnNo06"]);
                                    string UnNo07 = Modfunction.CheckNull(ja[i]["UnNo07"]);
                                    string UnNo08 = Modfunction.CheckNull(ja[i]["UnNo08"]);
                                    string UnNo09 = Modfunction.CheckNull(ja[i]["UnNo09"]);
                                    string UnNo10 = Modfunction.CheckNull(ja[i]["UnNo10"]);
                                    string Remark = Modfunction.CheckNull(ja[i]["Remark"]);
                                    LENGTH = Modfunction.ReturnZero(ja[i]["LENGTH"].ToString());
                                    WIDTH = Modfunction.ReturnZero(ja[i]["WIDTH"].ToString());
                                    HEIGHT = Modfunction.ReturnZero(ja[i]["HEIGHT"].ToString());
                                    GROSS_LB = Modfunction.ReturnZero(ja[i]["GROSS_LB"].ToString());
                                    IND_VOL =LENGTH * WIDTH * HEIGHT ;
                                    IND_VOL = Math.Round(IND_VOL / 166,2);
                                    VOL_LB = LENGTH * WIDTH * HEIGHT * GROSS_LB ;
                                    VOL_LB= Math.Round(VOL_LB / 166, 2);
                                    //VOL_LB = Double.Parse(VOL_LB.ToString("###,###,##0"),0);
                                    //VOL_LB = Double.Parse( string.Format("{#.##}", VOL_LB));
                                    if (GROSS_LB > IND_VOL)
                                    {
                                        IND_CHG_LB = Math.Round((GROSS_LB + 0.499999));
                                    }
                                    else
                                    {
                                        IND_CHG_LB = Math.Round((IND_VOL + 0.499999));
                                    }
                                    strSql = "insert into OH_PID_D( " +
                                              "  onhand_no," +
                                              "  LineItemNo, " +
                                              "  PACK_TYPE," +
                                              "  PackTypeCode, " +
                                              "  ISSUER_CODE," +
                                              "  Date_ADDED, " +
                                              "  SP_IND, " +
                                              "  TRK_BILL_NO," +
                                              "  PID_NO," +
                                              "  UnNo," +
                                              "  LENGTH," +
                                              "  WIDTH," +
                                              "  HEIGHT," +
                                              "  PIECES,"+
                                              "  GROSS_LB ," +
                                              "  IND_VOL ," +
                                              "  VOL_LB ," +
                                              "  IND_CHG_LB ," +
                                              "  INV_NO, " +
                                              "  UnNo01, " +
                                              "  UnNo02, " +
                                              "  UnNo03, " +
                                              "  UnNo04, " +
                                              "  UnNo05, " +
                                              "  UnNo06, " +
                                              "  UnNo07, " +
                                              "  UnNo08, " +
                                              "  UnNo09, " +
                                              "  UnNo10, " +
                                              "  Remark " +                                            
                                              "  )" +
                                                  "values( " +
                                                  Modfunction.SQLSafeValue(OnhandNo) + " , " +
                                                  intMaxLineItemNo + "," +
                                                  Modfunction.SQLSafeValue(PACK_TYPE) + "," +
                                                  Modfunction.SQLSafeValue(getPackTypeCode( Modfunction.SQLSafeValue(PACK_TYPE))) + "," +
                                                  Modfunction.SQLSafeValue(strUserId) + "," +
                                                   " getdate() ," +
                                                  Modfunction.SQLSafeValue(strSP_Ind_YN) + "," +
                                                  Modfunction.SQLSafeValue(TRK_BILL_NO) + "," +
                                                  Modfunction.SQLSafeValue(PID_NO) + "," +
                                                  Modfunction.SQLSafeValue(UnNo) + "," +
                                                  LENGTH + "," +
                                                  WIDTH + "," +
                                                  HEIGHT + "," +
                                                  "1" + "," +
                                                  GROSS_LB + "," +
                                                  IND_VOL + "," +
                                                  VOL_LB + "," +
                                                  IND_CHG_LB + "," +
                                                  "''," +
                                                  Modfunction.SQLSafeValue(UnNo01) + "," +
                                                  Modfunction.SQLSafeValue(UnNo02) + "," +
                                                  Modfunction.SQLSafeValue(UnNo03) + "," +
                                                  Modfunction.SQLSafeValue(UnNo04) + "," +
                                                  Modfunction.SQLSafeValue(UnNo05) + "," +
                                                  Modfunction.SQLSafeValue(UnNo06) + "," +
                                                  Modfunction.SQLSafeValue(UnNo07) + "," +
                                                  Modfunction.SQLSafeValue(UnNo08) + "," +
                                                  Modfunction.SQLSafeValue(UnNo09) + "," +
                                                  Modfunction.SQLSafeValue(UnNo10) + "," +
                                                  Modfunction.SQLSafeValue(Remark) + "" +

                                                  ") ";
                                    db.ExecuteSql(strSql);

                                }

                            }
                        }
                        Result = 1;
                    }
                }

            }
            catch { throw; }
            return Result;
     
        }

        public List<ON_PID_D> ValidatePID_NO(ONHAND_D request)
        {

            List<ON_PID_D> Result = null;

            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {

                    string PID_NO = request.strPID_NO; 

                    if (PID_NO != "")
                    {
                        string strSql = "Select  top 1 PID_NO, ONHAND_NO From OH_PID_D Where PID_NO =" + Modfunction .SQLSafeValue(PID_NO) + "";

                        Result= db.Select<ON_PID_D>(strSql);
                    }

                                   
                }

            }
            catch { throw; }
            return Result;
        }


        public List<ON_PID_D> CheckAlreadyPID(ONHAND_D request)
        {

            List<ON_PID_D> Result = null;

            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {

                    string strONHAND_NO = request.strONHAND_NO;

                    if (strONHAND_NO != "")
                    {
                        string strSql = "Select   Onhand_No  From OH_PID_D Where Onhand_No ='" + strONHAND_NO + "'";

                        Result = db.Select<ON_PID_D>(strSql);
                    }


                }

            }
            catch { throw; }
            return Result;
        }


        public List<ONHAND_D_Table> getTrk_Code( string onhandNo)
        {

            List<ONHAND_D_Table> Result = null;

            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {

                    string ONHAND_NO = onhandNo;

                    if (ONHAND_NO != "")
                    {
                        string strSql = "Select  TRK_CODE  From ONHAND_D Where ONHAND_NO ='" + ONHAND_NO + "'";

                        Result = db.Select<ONHAND_D_Table>(strSql);
                   
                    }


                }

            }
            catch { throw; }
            return Result;
        }



        public List<ON_PID_D> ValidateTRK_BILL_NO(ONHAND_D request)
        {

            List<ON_PID_D> Result = null;
            List<ONHAND_D_Table> Result_Onhand_D = null;
            string ResultTruckerBillNo = "";
            int ReturnTruckerBillNo = 0;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {

                    string TRK_BILL_NO = request.strTRK_BILL_NO;
                    string OnhandNo = request.strONHAND_NO;
                    string strTRK_BILL_NO ="";
                    string Trk_Code = "";
                    if (TRK_BILL_NO != "")
                    {
                        string strSql = "Select  top 1 TRK_BILL_NO, ONHAND_NO From OH_PID_D Where TRK_BILL_NO =" +     Modfunction.SQLSafeValue (TRK_BILL_NO) + "";
                        Result = db.Select<ON_PID_D>(strSql);

                        Result_Onhand_D = getTrk_Code(OnhandNo);
                        if (Result_Onhand_D.Count > 0)
                        {
                            Trk_Code =  Modfunction .CheckNull (Result_Onhand_D[0].TRK_CODE) ;
                            if (Trk_Code.Length >=3)
                            Trk_Code= Trk_Code.Substring(0,3);
                        }

                        if ( (Result.Count < 1) && (Trk_Code.ToUpper() =="FED")  && TRK_BILL_NO.Length >=12 )
                        {
                            if (TRK_BILL_NO.Length > 15)
                            {
                                strTRK_BILL_NO = TRK_BILL_NO.Substring(TRK_BILL_NO.Length - 15, 15);
                                ResultTruckerBillNo = db.SqlScalar<string>("EXEC verify_TruckerBillNo @TruckerBillNo", new { TruckerBillNo = strTRK_BILL_NO });
                                if (ResultTruckerBillNo == "Y")
                                {
                                    ReturnTruckerBillNo = 15;
                                }
                                else
                                {
                                    strTRK_BILL_NO = TRK_BILL_NO.Substring(TRK_BILL_NO.Length - 12, 12);
                                    ResultTruckerBillNo = db.SqlScalar<string>("EXEC verify_TruckerBillNo @TruckerBillNo", new { TruckerBillNo = strTRK_BILL_NO });                            
                                    ReturnTruckerBillNo = 12;
                                }

                            }
                            else if (TRK_BILL_NO.Length > 12 && TRK_BILL_NO.Length < 15)
                            {
                                strTRK_BILL_NO = TRK_BILL_NO.Substring(TRK_BILL_NO.Length - 12, 12);
                                ResultTruckerBillNo = db.SqlScalar<string>("EXEC verify_TruckerBillNo @TruckerBillNo", new { TruckerBillNo = strTRK_BILL_NO });
                                ReturnTruckerBillNo =12;
                           
                            }
                            else if (TRK_BILL_NO.Length == 12 || TRK_BILL_NO.Length == 15)
                            {
                                ResultTruckerBillNo = "Y";
                            }
                            else
                            {

                            }
                            ON_PID_D pid;
                            pid = new ON_PID_D();
                            pid.ResultTruckerBillNo = ResultTruckerBillNo;
                            pid.ReturnTruckerBillNo = ReturnTruckerBillNo;
                            Result.Add(pid);

                        }
                    }


                }

            }
            catch { throw; }
            return Result;
        }

        public string ConfirmAll_ONHAND_D(ONHAND_D request)
        {
          
            string  Result ="";
            String KeyOnhandNo = "";
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (request.UpdateAllString != null && request.UpdateAllString != "")
                    {
                        JArray ja = (JArray)JsonConvert.DeserializeObject(request.UpdateAllString);
                        if (ja != null)
                        {

                            for (int i = 0; i < ja.Count(); i++)
                            {
                              
                                string strSql = "";
                                string SHP_MODE = "";
                                string OH_STAT = "";
                                string Domestic = ja[i]["DomesticFlag"].ToString();
                                string  strAog = ja[i]["AOG"].ToString();
                                if (Domestic == "Y")
                                {      
                                    if (strAog == "Y")
                                    {
                                        SHP_MODE = "L1";
                                    }
                                    else
                                    {

                                        SHP_MODE = "L2";
                                    }
                                    OH_STAT = "DT";
                                }
                                else
                                {
                                    if (strAog == "Y")
                                    {
                                        SHP_MODE = "A1";
                                    }
                                    else
                                    {

                                        SHP_MODE = "A2";
                                    }
                                    OH_STAT = "RI";

                                }
                                KeyOnhandNo = generateOnhandNo(Domestic);                  
                                string SHP_CODE = ja[i]["SHP_CODE"].ToString();
                                string CNG_CODE = ja[i]["CNG_CODE"].ToString();
                                string ONHAND_date = ja[i]["ONHAND_date"].ToString();
                                string CASE_NO  =ja[i]["CASE_NO"].ToString();                                                           
                                string PUB_YN = ja[i]["PUB_YN"].ToString();
                                string HAZARDOUS_YN = ja[i]["HAZARDOUS_YN"].ToString();
                                string CLSF_YN = ja[i]["CLSF_YN"].ToString();
                                string ExerciseFlag = ja[i]["ExerciseFlag"].ToString();
                                string LOC_CODE = Modfunction.CheckNull(ja[i]["LOC_CODE"]);
                                string TRK_CODE = Modfunction.CheckNull(ja[i]["TRK_CODE"]);
                                string TRK_CHRG_TYPE = ja[i]["TRK_CHRG_TYPE"].ToString();
                                string PICKUP_SUP_datetime = ja[i]["PICKUP_SUP_datetime"].ToString();

                                if (PICKUP_SUP_datetime == "")
                                {
                                    PICKUP_SUP_datetime = null;
                                }
                                int NO_INV_WH ;
                                string UserID = ja[i]["UserID"].ToString();
                                if (ja[i]["NO_INV_WH"].ToString() == "")
                                {
                                    NO_INV_WH = 0;
                                }
                                else
                                {
                                    NO_INV_WH = int.Parse(ja[i]["NO_INV_WH"].ToString());
                                }

                                strSql = "insert into ONHAND_D( " +
                               "   onhand_no,"+
                               "   SHP_CODE," +
                               "   CNG_CODE ," +
                               "   SHP_MODE , " +
                               "   ONHAND_date," +                              
                               "   CASE_NO ," +
                               "   PUB_YN," +
                               "   HAZARDOUS_YN ," +
                               "   CLSF_YN ," +
                               "   ExerciseFlag ," +
                               "   LOC_CODE ," +
                               "   TRK_CODE ," +
                               "   OH_STAT  ," +
                               "   TRK_CHRG_TYPE ," +
                               "   PICKUP_SUP_datetime," +
                               "   TRK_DEL_datetime," +
                               "   NO_INV_WH," +
                               "   NO_DOC_INV, " +
                               "   Prate_YN, " +
                               "   Sector, " +
                               "   CargoPickupSector, " +
                               "   Zip_code, " +                                                 
                               "   Cargopickupzipcode, " +
                               "   Shippername," +
                               "   shipperaddress1," +
                               "   shipperaddress2," +
                               "   shipperaddress3," +
                               "   shipperaddress4," +
                               "   Consigneename," +
                               "   ConsigneeAddress1," +
                               "   ConsigneeAddress2," +
                               "   ConsigneeAddress3," +
                               "   ConsigneeAddress4," +
                               "   CreateBy," +
                               "   UpdateBy," +
                               "   CreateDateTime," +
                               "   UpdateDateTime," +
                               "   StatusCode " +
                               "  )" +
                                   "values( " +
                                   Modfunction.SQLSafeValue(KeyOnhandNo) +" , " +
                                   Modfunction.SQLSafeValue(SHP_CODE)+","+
                                   Modfunction.SQLSafeValue(CNG_CODE) + "," +
                                   Modfunction.SQLSafeValue(SHP_MODE) + "," +
                                   Modfunction.SQLSafeValue(ONHAND_date) + "," +
                                   Modfunction.SQLSafeValue(CASE_NO) + "," +
                                   Modfunction.SQLSafeValue(PUB_YN) + "," +
                                   Modfunction.SQLSafeValue(HAZARDOUS_YN) + "," +
                                   Modfunction.SQLSafeValue(CLSF_YN) + "," +
                                   Modfunction.SQLSafeValue(ExerciseFlag) + "," +
                                   Modfunction.SQLSafeValue(LOC_CODE) + "," +
                                   Modfunction.SQLSafeValue(TRK_CODE) + "," +
                                   Modfunction.SQLSafeValue(OH_STAT) + "," +                              
                                   Modfunction.SQLSafeValue(TRK_CHRG_TYPE) + "," +
                                   Modfunction.SQLSafeValue(PICKUP_SUP_datetime) + "," +
                                   Modfunction.SQLSafeValue(PICKUP_SUP_datetime) + "," +
                                   NO_INV_WH + "," +
                                   NO_INV_WH + "," +
                                   "'N'," +
                                  "( Select sectorcode  From Rcbp1  Where businesspartycode='" + SHP_CODE + "')," +
                                  "( Select sectorcode  From Rcbp1  Where businesspartycode='" + SHP_CODE + "')," +
                                  "( Select postalcode  From Rcbp1  Where businesspartycode='" + SHP_CODE + "'),"+
                                  "( Select postalcode  From Rcbp1  Where businesspartycode='" + SHP_CODE + "')," +
                                  "( Select BusinessPartyName  From Rcbp1  Where businesspartycode='" + SHP_CODE + "')," +
                                  "( Select Address1  From Rcbp1  Where businesspartycode='" + SHP_CODE + "')," +
                                  "( Select Address2  From Rcbp1  Where businesspartycode='" + SHP_CODE + "')," +
                                  "( Select Address3  From Rcbp1  Where businesspartycode='" + SHP_CODE + "')," +
                                  "( Select Address4  From Rcbp1  Where businesspartycode='" + SHP_CODE + "')," +
                                  "( Select BusinessPartyName  From Rcbp1  Where businesspartycode='" + CNG_CODE + "')," +
                                  "( Select Address1  From Rcbp1  Where businesspartycode='" + CNG_CODE + "')," +
                                  "( Select Address2  From Rcbp1  Where businesspartycode='" + CNG_CODE + "')," +
                                  "( Select Address3  From Rcbp1  Where businesspartycode='" + CNG_CODE + "')," +
                                  "( Select Address4  From Rcbp1  Where businesspartycode='" + CNG_CODE + "')," +
                                   Modfunction.SQLSafeValue(UserID) + "," +
                                   Modfunction.SQLSafeValue(UserID) + "," +
                                   "GETDATE()," +
                                   "GETDATE()," +
                                    "'USE'" +
                                   ") ";
                                db.ExecuteSql(strSql);
                            }
                            }
                            Result = KeyOnhandNo;
                        }
                    }
                
            }
            catch { throw; }
            return Result;
        }

         

        public string Update_ONHAND_D(ONHAND_D request)
        { 


            string Result = "";         
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (request.UpdateAllString != null && request.UpdateAllString != "")
                    {
                        JArray ja = (JArray)JsonConvert.DeserializeObject(request.UpdateAllString);
                        if (ja != null)
                        {

                            for (int i = 0; i < ja.Count(); i++)
                            {

                                string strSql = "";
                                string SHP_MODE = "";
                                string ONHAND_NO = ja[i]["ONHAND_NO"].ToString();
                                string SHP_CODE = ja[i]["SHP_CODE"].ToString();
                                string CNG_CODE = ja[i]["CNG_CODE"].ToString();
                                string Domestic = ja[i]["DomesticFlag"].ToString();
                                string strAog = ja[i]["AOG"].ToString(); 
                                string ONHAND_date = ja[i]["ONHAND_date"].ToString();
                                string CASE_NO = ja[i]["CASE_NO"].ToString();
                                string PUB_YN = ja[i]["PUB_YN"].ToString();
                                string HAZARDOUS_YN = ja[i]["HAZARDOUS_YN"].ToString();
                                string CLSF_YN = ja[i]["CLSF_YN"].ToString();
                                string ExerciseFlag = ja[i]["ExerciseFlag"].ToString();
                                string LOC_CODE = Modfunction.CheckNull(ja[i]["LOC_CODE"]);
                                string TRK_CODE = Modfunction.CheckNull(ja[i]["TRK_CODE"]);
                                string TRK_CHRG_TYPE = ja[i]["TRK_CHRG_TYPE"].ToString();
                                string PICKUP_SUP_datetime = ja[i]["PICKUP_SUP_datetime"].ToString();
                                if (PICKUP_SUP_datetime == "")
                                {
                                    PICKUP_SUP_datetime = null;
                                }
                                int NO_INV_WH;
                                string UserID = ja[i]["UserID"].ToString();
                                if (ja[i]["NO_INV_WH"].ToString() == "")
                                {
                                    NO_INV_WH = 0;
                                }
                                else
                                {
                                    NO_INV_WH = int.Parse(ja[i]["NO_INV_WH"].ToString());
                                }


                                if (Domestic == "Y")
                                {
                                    if (strAog == "Y")
                                    {
                                        SHP_MODE = "L1";
                                    }
                                    else
                                    {

                                        SHP_MODE = "L2";
                                    }

                                }
                                else
                                {
                                    if (strAog == "Y")
                                    {
                                        SHP_MODE = "A1";
                                    }
                                    else
                                    {

                                        SHP_MODE = "A2";
                                    }


                                }


                                strSql = "Update  ONHAND_D   Set" +
                               "   SHP_CODE ='" +SHP_CODE+ "'," +
                               "   CNG_CODE ='" + CNG_CODE + "'," +
                                 "   SHP_MODE ='" + SHP_MODE + "'," +
                               "   ONHAND_date ='" + ONHAND_date + "'," +
                               "   CASE_NO =" +   Modfunction.SQLSafeValue(CASE_NO)  + "," +
                               "   PUB_YN ='" + PUB_YN + "'," +
                               "   HAZARDOUS_YN  ='" + HAZARDOUS_YN + "'," +
                               "   CLSF_YN  ='" + CLSF_YN + "'," +
                               "   ExerciseFlag  ='" + ExerciseFlag + "'," +
                               "   LOC_CODE  ='" + LOC_CODE + "'," +
                               "   TRK_CODE  ='" + TRK_CODE + "'," +
                               "   TRK_CHRG_TYPE  ='" + TRK_CHRG_TYPE+ "'," +
                               "   PICKUP_SUP_datetime =" + Modfunction.SQLSafeValue(PICKUP_SUP_datetime) + "," +
                               "   TRK_DEL_datetime  ='" + PICKUP_SUP_datetime + "'," +
                               "   NO_INV_WH =" +NO_INV_WH + "," +
                               "    NO_DOC_INV =" + NO_INV_WH + "," +
                               "   Sector=  (Select sectorcode  From Rcbp1  Where businesspartycode = '" + SHP_CODE + "')," +
                               "   CargoPickupSector=  (Select sectorcode  From Rcbp1  Where businesspartycode = '" + SHP_CODE + "')," +
                                "   Zip_code=  (Select postalcode  From Rcbp1  Where businesspartycode = '" + SHP_CODE + "')," +
                               "   Cargopickupzipcode=  (Select postalcode  From Rcbp1  Where businesspartycode = '" + SHP_CODE + "')," +
                               "   Shippername=  (Select BusinessPartyName  From Rcbp1  Where businesspartycode = '" + SHP_CODE + "')," +
                               "   shipperaddress1=  (Select Address1  From Rcbp1  Where businesspartycode = '" + SHP_CODE + "')," +
                               "   shipperaddress2=  (Select Address2   From Rcbp1  Where businesspartycode = '" + SHP_CODE + "')," +
                               "   shipperaddress3=  (Select Address3  From Rcbp1  Where businesspartycode = '" + SHP_CODE + "')," +
                               "   shipperaddress4=  (Select Address4  From Rcbp1  Where businesspartycode = '" + SHP_CODE + "')," +
                               "   Consigneename=  (Select BusinessPartyName  From Rcbp1  Where businesspartycode = '" + CNG_CODE + "')," +
                               "   ConsigneeAddress1=  (Select Address1  From Rcbp1  Where businesspartycode = '" + CNG_CODE + "')," +
                               "   ConsigneeAddress2=  (Select Address2  From Rcbp1  Where businesspartycode = '" + CNG_CODE + "')," +
                               "   ConsigneeAddress3=  (Select Address3  From Rcbp1  Where businesspartycode = '" + CNG_CODE + "')," +
                               "   ConsigneeAddress4=  (Select Address4  From Rcbp1  Where businesspartycode = '" + CNG_CODE + "')," +
                               "   UpdateBy ='" + UserID + "'," +
                               "   UpdateDateTime =GETDATE() " +
                               "   WHERE ONHAND_NO ='" + ONHAND_NO+ "'"+
                               "";

                                db.ExecuteSql(strSql);
                            }
                        }
                        Result ="1";
                    }
                }

            }
            catch { throw; }
            return Result;
        }

        private string getPackTypeCode(string packType)
        {
            string PackTypeCode = "";
            using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
            {
                try
                {
                    string strSql = "SELECT  ISNULL(PackTypeCode,'') AS  PackTypeCode FROM Rcpk1 WHERE PackType = " + packType + "";
                    List<Rcpk1> Rcpk1 = db.Select<Rcpk1>(strSql);

                    PackTypeCode = Modfunction.CheckNull(Rcpk1[0].PackTypeCode);
                  

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return PackTypeCode;
        }
        private string generateOnhandNo(string Domestic)
        {
            ONHAND_D pair;
            var prefixRules = "";
            var runningNo = "";

            using (var db = DbConnectionFactory.OpenDbConnection("WMS"))
            {
                try
                {
                    string strWhere = "";
                    if (Domestic == "Y")
                    {
                        strWhere = "And JobType ='DT'";
                    }
                    List<sanm1> sanm1 = db.Select<sanm1>("SELECT Prefix, NextNo FROM sanm1 WHERE NumberType = 'Onhand_D' "+ strWhere + "");

                
                        prefixRules = Modfunction.CheckNull(sanm1[0].Prefix);
                        runningNo = Modfunction.CheckNull(sanm1[0].NextNo);
                        pair = generateTransactionNo(prefixRules, runningNo);
                        string strSql = "";
                        strSql = "NextNo=" + Modfunction.SQLSafeValue(pair.NextNo) + "";
                        db.Update("sanm1",
                                      strSql,
                                      "numbertype='Onhand_D' ");
                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return pair.TrxNo;
        }


        private ONHAND_D generateTransactionNo(string prefixRules, string runningNo)
        {
          
                var pair = new ONHAND_D();
                var rules = prefixRules.Split(',');
                var now = DateTime.Now;

                foreach (var r in rules)
                {
                    var rule = r.Trim();
                    if (rule == "YY" || rule == "MM")
                    {
                        pair.TrxNo += now.ToString(rule);
                    }
                    else
                    {
                        //assume is Fxx, until further instruction
                        pair.TrxNo += rule.Substring(1);
                    }
                }

                pair.TrxNo += runningNo;

                //compute next number for storage.
                var runningInt = 0;
                Int32.TryParse(runningNo, out runningInt);
                ++runningInt;
                pair.NextNo = runningInt.ToString(new String('0', runningNo.Length));

                return pair;
            
         
        }
    }
   

}
