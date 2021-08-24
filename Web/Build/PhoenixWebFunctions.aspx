<%@ Page Language="C#" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.PlannedMaintenance" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Purchase" %>
<%@ Import Namespace="SouthNests.Phoenix.DryDock" %>
<%@ Import Namespace="SouthNests.Phoenix.DocumentManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewOffshore" %>
<%@ Import Namespace="SouthNests.Phoenix.Dashboard" %>
<%@ Import Namespace="SouthNests.Phoenix.StandardForm" %>
<%@ Import Namespace="Newtonsoft.Json.Linq" %>
<script language="c#" runat="server">

    private string strFunctionName;
    private string[] arrNames;
    private string[] arrValues;

    protected void Page_Load(object o, EventArgs e)
    {
        Session.Timeout = 120;
        GetParameters(Server.UrlDecode((new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd()));

        if (strFunctionName.ToUpper().Equals("SELECTJOB"))
        {
            int ischecked = arrValues[2].ToUpper().Equals("TRUE") ? 1 : 0;
            int jobregister = int.Parse(arrValues[3].ToString());
            PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    jobregister, General.GetNullableGuid(arrValues[1]), ischecked);

        }
        if (strFunctionName.ToUpper().Equals("SELECTTARIFF"))
        {
            int ischecked = arrValues[2].ToUpper().Equals("TRUE") ? 1 : 0;
            int jobregister = int.Parse(arrValues[3].ToString());
            PhoenixDryDockStandardCost.UpdateDryDockStandardUnitVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    jobregister, General.GetNullableGuid(arrValues[1]), ischecked);

        }
        if (strFunctionName.ToUpper().Equals("SELECTJOBDETAIL"))
        {
            int ischecked = arrValues[2].ToUpper().Equals("TRUE") ? 1 : 0;
            PhoenixDryDockJobGeneral.UpdateDryDockJobGeneralDetailVessel(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    General.GetNullableGuid(arrValues[1]).Value, ischecked);

        }

        if (strFunctionName.ToUpper().Equals("JOBFORMMAP"))
        {
            string jobid = arrValues[1].ToString();
            string formid = arrValues[2].ToString();
            int verifiedyn = arrValues[3].ToUpper().Equals("TRUE") ? 1 : 0;
            PhoenixPlannedMaintenanceJobHistoryTemplateList.PMSHistoryTemplateUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                          , new Guid(formid)
                                                                          , verifiedyn
                                                                          , new Guid(jobid));

        }
        if(strFunctionName.ToUpper().Equals("MEASUREUPDATE"))
        {
            string measurecode = arrValues[1].ToString();
            int verifiedyn = arrValues[2].ToUpper().Equals("TRUE") ? 1 : 0;
            int usercode = int.Parse(arrValues[3].ToString());
            PhoenixDashboardOption.UserMeasureListUpdate(usercode,measurecode,verifiedyn);
        }
        if(strFunctionName.ToUpper().Equals("MODULEUPDATE"))            //Dashboard
        {
            int Moduleid = int.Parse(arrValues[1].ToString());
            int verifiedyn = arrValues[2].ToUpper().Equals("TRUE") ? 1 : 0;
            int usercode = int.Parse(arrValues[3].ToString());
            PhoenixDashboardOption.UserModuleListUpdate(usercode,Moduleid,verifiedyn);
        }

        if(strFunctionName.ToUpper().Equals("DASHBOARDREFRESH"))
        {
            PhoenixDashboardOption.DashboardRefresh();
        }
        //      private void CrewListJson()
        //{
        //    Response.AddHeader("Content-Type", "application/json");
        //    var parameter = arrValues[1].ToString();
        //    string value = PhoenixFormBuilder.FetchValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, "CREWLIST", parameter);
        //    Response.Write(value);
        //}
        //DataSet ds = PhoenixFormBuilder.CompanyList(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        //string value = ds.Tables[0].Rows[0]["FLDCOMPANYNAME"].ToString();
        if (strFunctionName.ToUpper().Equals("VALIDATEUSER"))
        {

            Response.AddHeader("Content-Type", "application/json");
            string strJsonData = arrValues[1].ToString();
            string sign = arrValues[2].ToString();
            string failedjson = string.Empty;
            JObject fbjson = JObject.Parse(strJsonData);
            JArray array = new JArray();
            DataTable rank = PhoenixFormBuilder.UserFetchDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            string loginrank = string.Empty;
            if (rank.Rows.Count > 0)
            {
                loginrank = rank.Rows[0]["FLDRANKCODE"].ToString().ToLower();
            }
            foreach (var token in fbjson.Properties().Where(p => p.Name.Contains("verify")))
            {
                string[] numbers = Regex.Split(token.Name, @"\D+");
                string val = "0";
                //var parameter = arrValues[].ToString();
                //DataSet ds1 = PhoenixFormBuilder.FetchValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, "CREWLIST", parameter);
                if (numbers.Length > 0)
                {
                    val = numbers[1];
                    if (fbjson["username" + val] != null)
                    {
                        string employeeid = fbjson["username" + val].ToString();
                        string password = fbjson["password" + val].ToString();
                        string signature = fbjson["password" + val].ToString();

                        if (!string.IsNullOrEmpty(employeeid))
                        {
                            DataTable dt = new DataTable();
                            try
                            {
                                dt = PhoenixFormBuilder.FormBuilderLoginValidate(int.Parse(employeeid), password);

                                if (dt.Rows.Count > 0)
                                {
                                    signature = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
                                }
                            }
                            catch (Exception ex)
                            {
                                //array.Add(new JProperty("validation",)
                                signature = ex.Message;
                            }
                        }
                        fbjson["signature" + val] = signature;
                    }
                    else
                    {
                        var r = fbjson[token.Name].Value<string>();
                        foreach (string t in sign.Split('`'))
                        {
                            string signature = fbjson["signature" + val].ToString();
                            if (r.ToLower() == "true" && !signature.Contains("-"))
                            //if (r.ToLower() == "true" && !fbjson["signature" + val].Contains("-"))
                            {
                                if (t.Contains(token.Name + "~"))
                                {
                                    if (t.ToLower().Contains(",all,") || t.ToLower().Contains("," + loginrank + ","))
                                    {
                                        fbjson["signature" + val] = PhoenixSecurityContext.CurrentSecurityContext.FirstName + " " + PhoenixSecurityContext.CurrentSecurityContext.LastName;
                                    }
                                    else
                                    {
                                        fbjson["signature" + val] = "You are not authorized to sign.";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Response.Write(fbjson.ToString());
        }
        if (strFunctionName.ToUpper().Equals("REPORTINSERT"))
        {
            try
            {
                Guid reportIdNew = Guid.Empty;
                string formid = arrValues[1].ToString();
                String strJsonData = arrValues[2].ToString();
                string reportid = arrValues[3].ToString();
                string signature = arrValues[5].ToString();
                string revid = arrValues[6].ToString();
                //string status = arrValues[4].ToString();
                //string formtype = arrValues[4].ToString();
                //string username = arrValues[4].ToString();
                //string password = arrValues[5].ToString();
                StringBuilder myStringBuilder = new StringBuilder(strJsonData);
                string seprator = "data:image/png;base64,";
                List<int> indexes = new List<int>();
                for (int index = 0; ; index += seprator.Length)
                {
                    index = strJsonData.IndexOf(seprator, index);
                    if (index == -1)
                        break;
                    indexes.Add(index);
                }
                foreach (int y in indexes)
                {
                    myStringBuilder.Replace(" ", "&plus;", y, strJsonData.IndexOf("\"", y) - y);
                }
                JObject fbjson = JObject.Parse(myStringBuilder.ToString());
                string signaturedone = string.Empty;
                var d = fbjson.SelectToken("$..data");
                var data = JObject.Parse(d.ToString());
                //string val = "0";
                //string isSignature = data["verify" + val].ToString();
                foreach (var token in data.Properties().Where(p => p.Name.StartsWith("verify")))
                {
                    string[] numbers = Regex.Split(token.Name, @"\D+");
                    string val = "0";

                    if (numbers.Length > 0)

                    {
                        val = numbers[1];
                        //string employeeid = data["username" + val].ToString();
                        //string password = data["password" + val].ToString();
                        //string isSignature = data["signature" + val].ToString();
                        string isSignature = data["verify" + val].ToString();
                        //string isSignature = "";
                        //if (isSignature.Length > 0 && !(isSignature.ToUpper().Contains("INVALID") && isSignature.ToUpper().Contains("LOGIN")))
                        if (isSignature.Length > 0 && (isSignature.ToUpper().Contains("TRUE")))
                        {
                            // signaturedone += "#username" + val + "#`";
                            signaturedone += "#verify" + val + "#`";
                        }
                    }
                }
                var state = "1";
                foreach (var token in data.Properties().Where(p => p.Name.ToLower().StartsWith("saveasdraft")))
                {
                    string[] numbers = Regex.Split(token.Name, @"\D+");
                    string val = "";
                    if (numbers.Length > 0)
                    {
                        val = numbers[1];
                        string draft = data["saveAsDraft" + val].ToString();
                        if (draft.ToLower() == "true")
                            state = "1";
                    }
                }
                foreach (var token in data.Properties().Where(p => p.Name.StartsWith("pendingApproval")))
                {
                    string[] numbers = Regex.Split(token.Name, @"\D+");
                    string val = "";
                    if (numbers.Length > 0)
                    {
                        val = numbers[1];
                        string draft = data["pendingApproval" + val].ToString();
                        if (draft.ToLower() == "true")
                            state = "10";
                    }
                }
                //foreach (var token in data.Properties().Where(p => p.Name.StartsWith("submit")))
                //{
                //    string[] numbers = Regex.Split(token.Name, @"\D+");
                //    string val = "";
                //    if (numbers.Length > 0)
                //    {
                //        val = numbers[1];
                //        string submit = data["submit" + val].ToString();
                //        if (submit.ToLower() == "true")
                //            state = "3";
                //    }
                //}
                //if data.Properties().Where(p => p.Name.StartsWith("verify")
                //if (data.Properties().Contains("submit"))
                //{

                //}
                //if (fbjson["submit"] != null)
                //{
                signaturedone = signaturedone.TrimEnd('`');
                //    var t1 = fbjson.Properties().Where(p => p.Name.Contains("submit1"));
                //if (signaturedone != string.Empty && t1!= null)
                //{

                string[] pendingsign = signature.Split('`');
                string temp = string.Empty;
                foreach (string p in pendingsign)
                {
                    bool done = false;
                    if (p.Equals("")) continue;
                    foreach (string t in signaturedone.Split('`'))
                    {
                        string[] x = p.Replace("#", "").Split('~');
                        if (t.Replace("#", "").Equals(x[0]))
                        {
                            done = true;
                            break;
                        }
                    }
                    if (!done)
                    {
                        string[] x = p.Replace("#", "").Split('~');
                        if (temp.IndexOf(x[1]) < 0)
                            temp += x[1].TrimEnd(',');
                    }
                }





                var t1 = fbjson.Properties().Where(p => p.Name.Contains("submit1"));
                // t1 = var token in data.Properties().Where(p => p.Name.StartsWith("submit1"))
                // if (signaturedone != string.Empty && (t1=true))
                //var t1 = fbjson.Properties().Where(p => p.Name.Contains("submit1"));
                if (signaturedone != string.Empty)

                //if (signaturedone != string.Empty && (  fbjson.Properties().Where(p => p.Name.Contains("verify")))
                {
                    state = "9";
                }


                foreach (var token in data.Properties().Where(p => p.Name.StartsWith("submit")))
                {
                    string[] numbers = Regex.Split(token.Name, @"\D+");
                    string val = "";
                    if (numbers.Length > 0)
                    {
                        val = numbers[1];
                        string submit = data["submit" + val].ToString();
                        if (submit.ToLower() == "true" && !(strJsonData.ToUpper().Contains("YOU ARE NOT AUTHORIZED TO SIGN")))
                            state = "3";
                    }
                }

                if (temp.Length > 0 && signaturedone != string.Empty)
                {
                    string[] x = temp.Trim(',').Split(',');
                    state = "8";
                    foreach (string s in x)
                    {
                        if (s.ToUpper() != "MST")
                        {
                            state = "9";
                            break;
                        }
                    }
                }
                // }

                PhoenixFormBuilder.ReportInsert(new Guid(formid), General.GetNullableGuid(reportid), fbjson["data"].ToString(), int.Parse(state), ref reportIdNew
                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID, signaturedone, temp, General.GetNullableGuid(revid));
                string json = "{\"reportId\":\"" + reportIdNew + "\"}";

                Response.Write(json);

            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                Response.Write("{\"Error\":\"" + ex.Message + "\"}");
            }
        }

        if (strFunctionName == "CheckForAlert")
        {
            CheckForAlert();
        }

        if (strFunctionName == "TestMethod")
        {
            Response.Write(Server.UrlDecode((new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd()));
        }

        if (strFunctionName == "PutMyValues")
        {
            PutMyValues();
        }

        if (strFunctionName == "PickListSet")
        {
            PickListSet();
        }

        if (strFunctionName == "PickListAccountSet")
        {
            PickListAccountSet();
        }

        if (strFunctionName == "PickListSubAccountSet")
        {
            PickListSubAccountSet();
        }

        if (strFunctionName == "PickListGet")
        {
            PickListGet();
        }

        if (strFunctionName == "GetCurrentVessel")
        {
            GetCurrentVessel();
        }

        if (strFunctionName == "GetCurrentCompany")
        {
            GetCurrentCompany();
        }

        if (strFunctionName == "GridColumnReorderSet")
        {
            GridColumnReorderSet();
        }
        if (strFunctionName == "AttachmentSet")
        {
            AttachmentsNameValueSet();
        }
        if (strFunctionName == "AttachmentGet")
        {
            AttachmentsNameValueGet();
        }
        if (strFunctionName == "FileUploadSet")
        {
            FileUploadSet();
        }
        if (strFunctionName == "FileUploadGet")
        {
            FileUploadGet();
        }
        if (strFunctionName == "SetMenuCode")
        {
            SetMenuCode();
        }
        if (strFunctionName == "AuthenticateAccess")
        {
            AuthenticateAccess();
        }
        if (strFunctionName == "ConvertExchangeRate")
        {
            ConvertExchangeRate();
        }

        if (strFunctionName == "BulkSaveQuotationLine")
        {
            BulkSaveQuotationLine();
        }

        if (strFunctionName == "BulkSaveOrderLine")
        {
            BulkSaveOrderLine();
        }

        if (strFunctionName == "ApprovalRequest")
        {
            ApprovalRequestInsert();
        }
        if (strFunctionName == "SetCurrentDMSDocumentSelection")
        {
            SetCurrentDMSDocumentSelection();
        }
        if (strFunctionName == "SetCurrentDMSPickListSelection")
        {
            SetCurrentDMSPickListSelection();
        }
        if (strFunctionName == "PopulateDMSTreeOnDemand")
        {
            PopulateDMSTreeOnDemand();
        }

        if (strFunctionName == "PopulateNewDMSTreeOnDemand")
        {
            PopulateNewDMSTreeOnDemand();
        }
        if (strFunctionName == "BulkSaveOrderLineCompareScreen")
        {
            BulkSaveOrderLineCompareScreen();
        }
        if (strFunctionName == "SaveTankPlanLoad")
        {
            SaveTankPlanLoadConsumption();
        }
        if (strFunctionName.ToUpper() == "VAIDATETAB")
        {
            VaidateTab();
        }
        if (strFunctionName.ToUpper() == "FBVALUEFETCH")
        {
            FBValueFetch();
        }
        if (strFunctionName.ToUpper() == "CREWLISTJSON" || Request.QueryString != null && Request.QueryString["functionname"] == "CREWLISTJSON")
        {
            CrewListJson();
        }
        if (strFunctionName.ToUpper() == "FBPRINT")
        {
            PrintFB();
        }
        if (strFunctionName.ToUpper() == "APPROVALSTATUS")
        {
            ApprovalUpdate();
        }
        if(strFunctionName.ToUpper() == "SAVEGRIDFILTER")
        {
            SaveGriFilter();
        }
    }


    private void BulkSaveQuotationLine()
    {
        try
        {
            NameValueCollection nvc = new NameValueCollection();
            string quotationid, quotationlineids, quantities, prices, discounts, units, deliverydays;
            for (int i = 0; i < arrNames.Length; i++)
            {
                nvc.Add(arrNames[i], arrValues[i]);
            }


            quotationid = nvc.Get("hidQuotationId").ToString();
            quotationlineids = nvc.Get("hidQuotationLineId").ToString();
            quantities = nvc.Get("txtQuantityEdit").ToString();
            prices = nvc.Get("txtQuotedPriceEdit").ToString();
            discounts = nvc.Get("txtDiscountEdit").ToString();
            units = nvc.Get("ucUnit").ToString();
            units = units.Replace("Dummy", "");
            deliverydays = nvc.Get("txtDeliveryEdit").ToString();

            PhoenixPurchaseQuotationLine.UpdateQuotationLineBulk(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                quotationlineids,
                new Guid(quotationid),
                quantities,
                prices,
                discounts,
                units,
                deliverydays);

            Response.Write("Quotation line item details updated successfully.");
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void BulkSaveOrderLine()
    {
        try
        {
            NameValueCollection nvc = new NameValueCollection();
            string orderid, orderlineids, requestedqty, orderedqty, receivedqty, units;
            for (int i = 0; i < arrNames.Length; i++)
            {
                nvc.Add(arrNames[i], arrValues[i]);
            }

            orderid = nvc.Get("hidOrderId").ToString();
            orderlineids = nvc.Get("hidOrderLineId").ToString();
            requestedqty = nvc.Get("txtRequestedQuantityEdit").ToString();
            orderedqty = nvc.Get("txtOrderedQuantityEdit").ToString();
            receivedqty = nvc.Get("txtReceivedQuantityEdit").ToString();
            units = nvc.Get("ucUnit").ToString();
            units = units.Replace("Dummy", "");

            PhoenixPurchaseOrderLine.UpdateOrderLineBulk(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                orderlineids,
                new Guid(orderid),
                requestedqty,
                orderedqty,
                receivedqty,
                units);

            Response.Write("Order line item details updated successfully.");
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    private void BulkSaveOrderLineCompareScreen()
    {
        try
        {
            NameValueCollection nvc = new NameValueCollection();
            string orderid, orderlineids, orderedqty,vesselid;
            for (int i = 0; i < arrNames.Length; i++)
            {
                nvc.Add(arrNames[i], arrValues[i]);
            }

            orderid = nvc.Get("hidOrderId").ToString();
            orderlineids = nvc.Get("hidOrderLineId").ToString();
            orderedqty = nvc.Get("txtOrderedQuantity").ToString();
            vesselid = nvc.Get("hidVesselId").ToString();

            PhoenixPurchaseOrderLine.UpdateOrderLineBulkFromCompareScreen(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                orderlineids,
                new Guid(orderid),
                int.Parse(vesselid),
                orderedqty
                );

            Response.Write("Approved quantity updated successfully.");
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void PickListSet()
    {
        NameValueCollection nvc = new NameValueCollection();
        for (int i = 0; i < arrNames.Length; i++)
        {
            nvc.Add(arrNames[i], arrValues[i]);
        }
        Filter.CurrentPickListSelection = nvc;
    }

    private void PickListGet()
    {
        NameValueCollection nvc = Filter.CurrentPickListSelection;
        string args = "";

        for (int i = 0; i < nvc.Count; i++)
        {
            args += nvc.GetKey(i);
            args += "=";
            args += nvc.Get(nvc.GetKey(i));
            args += "|";
        }
        Response.Write(args);
    }

    private void PickListAccountSet()
    {
        NameValueCollection nvc = new NameValueCollection();
        for (int i = 0; i < arrNames.Length; i++)
        {
            nvc.Add(arrNames[i], arrValues[i]);
        }

        Filter.CurrentPickListSelection = nvc;

        DataSet ds = PhoenixRegistersAccount.AccountPickListSearch(
            PhoenixSecurityContext.CurrentSecurityContext.CompanyID
            , nvc.Get(nvc.GetKey(1))
            , nvc.Get(nvc.GetKey(2)));

        string args = "";

        if (ds.Tables[0].Rows.Count == 1)
        {
            DataTable dt = ds.Tables[0];

            nvc.Set(nvc.GetKey(1), dt.Rows[0]["FLDACCOUNTCODE"].ToString());
            nvc.Set(nvc.GetKey(2), dt.Rows[0]["FLDDESCRIPTION"].ToString());
            nvc.Set(nvc.GetKey(3), dt.Rows[0]["FLDACCOUNTID"].ToString());
            nvc.Set(nvc.GetKey(4), dt.Rows[0]["FLDACCOUNTSOURCE"].ToString());
            nvc.Set(nvc.GetKey(5), dt.Rows[0]["FLDACCOUNTUSAGE"].ToString());
            nvc.Set(nvc.GetKey(6), string.Empty);
            nvc.Set(nvc.GetKey(7), string.Empty);
            nvc.Set(nvc.GetKey(8), string.Empty);
            nvc.Set(nvc.GetKey(9), string.Empty);


            for (int i = 0; i < nvc.Count; i++)
            {
                args += nvc.GetKey(i);
                args += "=";
                args += nvc.Get(nvc.GetKey(i));
                args += "|";
            }
        }
        else
        {
            for (int i = 0; i < nvc.Count; i++)
            {
                args += nvc.GetKey(i);
                args += "=";
                args += "|";
            }
        }
        Response.Write(args);
    }

    private void PickListSubAccountSet()
    {
        NameValueCollection nvc = new NameValueCollection();
        for (int i = 0; i < arrNames.Length; i++)
        {
            nvc.Add(arrNames[i], arrValues[i]);
        }

        Filter.CurrentPickListSelection = nvc;

        DataSet dsaccount = null;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        ViewState["SelectedAccountId"] = "";
        ViewState["ACCOUNTUSAGE"] = "";

        if (nvc.Get(nvc.GetKey(3)) != null && nvc.Get(nvc.GetKey(3)).Length > 0)
        {
            ViewState["SelectedAccountId"] = General.GetNullableString(nvc.Get(nvc.GetKey(3)));
            Session["SelectedAccountId"] = ViewState["SelectedAccountId"];
            dsaccount = PhoenixRegistersAccount.EditAccount(Convert.ToInt32(ViewState["SelectedAccountId"].ToString()));
        }

        if (dsaccount != null)
        {
            if (dsaccount.Tables[0].Rows.Count > 0)
            {
                DataRow draccount = dsaccount.Tables[0].Rows[0];
                ViewState["ACCOUNTUSAGE"] = draccount["FLDACCOUNTUSAGE"].ToString();
            }
        }

        DataSet ds = PhoenixCommonRegisters.SubAccountSearch(
            General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
          , General.GetNullableInteger(ViewState["SelectedAccountId"].ToString())   //selectedaccid
          , General.GetNullableInteger(ViewState["ACCOUNTUSAGE"].ToString())        //accusage
          , General.GetNullableString(nvc.Get(nvc.GetKey(6)))                       //txtSubAccountCodeSearch.Text
          , General.GetNullableString(nvc.Get(nvc.GetKey(7)))                       //txtDescriptionSearch
          , "", null,
          1,
          10,
          ref iRowCount,
          ref iTotalPageCount);

        string args = "";

        if (dsaccount != null)
        {
            if (dsaccount.Tables[0].Rows.Count > 0)
            {
                DataRow draccount = dsaccount.Tables[0].Rows[0];
                nvc.Set(nvc.GetKey(1), draccount["FLDACCOUNTCODE"].ToString());
                nvc.Set(nvc.GetKey(2), draccount["FLDDESCRIPTION"].ToString());
                nvc.Set(nvc.GetKey(3), draccount["FLDACCOUNTID"].ToString());
                nvc.Set(nvc.GetKey(4), draccount["FLDACCOUNTSOURCENAME"].ToString());
                nvc.Set(nvc.GetKey(5), draccount["FLDACCOUNTUSAGENAME"].ToString());
            }
        }

        if (ds.Tables[0].Rows.Count == 1)
        {
            DataTable dt = ds.Tables[0];
            nvc.Set(nvc.GetKey(6), dt.Rows[0]["FLDSUBACCOUNTCODE"].ToString());
            nvc.Set(nvc.GetKey(7), dt.Rows[0]["FLDDESCRIPTION"].ToString());
            nvc.Set(nvc.GetKey(8), dt.Rows[0]["FLDSUBACCOUNTMAPID"].ToString());
            nvc.Set(nvc.GetKey(9), dt.Rows[0]["FLDDESCRIPTION"].ToString());

            for (int i = 0; i < nvc.Count; i++)
            {
                args += nvc.GetKey(i);
                args += "=";
                args += nvc.Get(nvc.GetKey(i));
                args += "|";
            }
        }
        else
        {
            for (int i = 0; i < nvc.Count; i++)
            {
                args += nvc.GetKey(i);
                args += "=";
                args += "|";
            }
        }
        Response.Write(args);
    }

    private void PutMyValues()
    {
        for (int i = 0; i < arrNames.Length; i++)
        {
            Session[arrNames[i]] = arrValues[i];
        }
    }

    private void GridColumnReorderSet()
    {
        NameValueCollection nvc = new NameValueCollection();
        for (int i = 0; i < arrNames.Length; i++)
        {
            nvc.Add(arrNames[i], arrValues[i]);
        }
        Filter.CurrentPickListSelection = nvc;
    }

    private void GetCurrentVessel()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext != null)
        {
            Response.Write(PhoenixSecurityContext.CurrentSecurityContext.FleetName + " / " +
                                PhoenixSecurityContext.CurrentSecurityContext.VesselName);
        }
    }

    private void GetCurrentCompany()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext != null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.CompanyID <= 0)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.UserDefaultCompanyId != null)
                {
                    PhoenixSecurityContext.CurrentSecurityContext.CompanyID = (int)PhoenixSecurityContext.CurrentSecurityContext.UserDefaultCompanyId;
                }
                PhoenixSecurityContext.CurrentSecurityContext.CompanyName = PhoenixSecurityContext.CurrentSecurityContext.UserDefaultCompanyName;
            }
            Response.Write("/" + PhoenixSecurityContext.CurrentSecurityContext.CompanyName + "," + Filter.CurrentAccountsFunctionSelected);
        }
    }

    private void GetParameters(string strQueryString)
    {
        string[] arrURLParameters;


        arrURLParameters = strQueryString.Split('|');
        arrNames = new string[arrURLParameters.Length];
        arrValues = new string[arrURLParameters.Length];

        if (!string.IsNullOrEmpty(strQueryString))
        {
            for (int i = 0; i < arrURLParameters.Length; i++)
            {
                string[] param = arrURLParameters[i].Split('=');
                arrNames[i] = param[0];
                arrValues[i] = (param.Length > 2) ? string.Join("=", param, 1, param.Length - 1) : param[1];
            }



            strFunctionName = arrValues[0];
        }
        else
        {
            strFunctionName = "";
        }
    }
    private void FileUploadSet()
    {
        NameValueCollection nvc = new NameValueCollection();
        for (int i = 1; i < arrNames.Length; i++)
        {
            nvc.Add(arrNames[i], arrValues[i]);
        }
        Filter.CurrentFileUploadSelection = nvc;
    }
    private void FileUploadGet()
    {
        NameValueCollection nvc = Filter.CurrentFileUploadSelection;
        string args = "";

        for (int i = 0; i < nvc.Count; i++)
        {
            args += nvc.GetKey(i);
            args += "=";
            args += nvc.Get(nvc.GetKey(i));
            args += "|";
        }
        Response.Write(args);
    }
    private void AuthenticateAccess()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext != null && arrValues.Length > 0)
        {
            Filter.CurrentAccountsFunctionSelected = arrValues[1] + "," + arrValues[2];
            if ((arrValues[1].Length > 0 && arrValues[1].StartsWith("Accounts/")) && arrValues[1] != "Accounts/AccountsInvoiceMaster.aspx")
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.CompanyID <= 0)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserDefaultCompanyId != null)
                    {
                        PhoenixSecurityContext.CurrentSecurityContext.CompanyID = (int)PhoenixSecurityContext.CurrentSecurityContext.UserDefaultCompanyId;
                    }
                    PhoenixSecurityContext.CurrentSecurityContext.CompanyName = PhoenixSecurityContext.CurrentSecurityContext.UserDefaultCompanyName;
                    Response.Write(arrValues[1]);
                }
                else
                {
                    Response.Write(arrValues[1]);
                }
            }
            else
                Response.Write(arrValues[1]);
        }
    }
    private void AttachmentsNameValueSet()
    {
        NameValueCollection nvc = new NameValueCollection();
        for (int i = 1; i < arrNames.Length; i++)
        {
            nvc.Add(arrNames[i], arrValues[i]);
        }
        Filter.CurrentAttachmentListSelection = nvc;
    }
    private void AttachmentsNameValueGet()
    {
        NameValueCollection nvc = Filter.CurrentAttachmentListSelection;
        string args = "";

        for (int i = 0; i < nvc.Count; i++)
        {
            args += nvc.GetKey(i);
            args += "=";
            args += nvc.Get(nvc.GetKey(i));
            args += "|";
        }
        Response.Write(args);
    }
    private void SetMenuCode()
    {
        //SessionUtil.CheckUrlAccess(arrValues[1], arrValues[2]);
        Filter.CurrentMenuCodeSelection = arrValues[1];
        Filter.SessionTimeoutTracker = 0;
    }
    private void CheckForAlert()
    {
        DataSet ds = PhoenixRegistersAlerts.CheckAlert(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        if (ds.Tables[0].Rows.Count > 0)
            Response.Write("1");
        else
            Response.Write("0");
    }
    private void ConvertExchangeRate()
    {

        NameValueCollection nvc = new NameValueCollection();
        for (int i = 0; i < arrNames.Length; i++)
        {
            nvc.Add(arrNames[i], arrValues[i]);
        }
        DataTable dt = new DataTable();
        dt = PhoenixRegistersExchangeRate.ConvertExchangeRate(decimal.Parse(arrValues[1].Replace("_", "0")));
        string args = "";
        var j = 0;
        for (int i = 1; i < nvc.Count; i++)
        {
            if (nvc.Get(nvc.GetKey(i)) == "OUT")
            {
                args += nvc.GetKey(i);
                args += "=";
                args += dt.Rows[0].ItemArray[j].ToString();
                args += "|";
                j++;
            }
        }
        Response.Write(args);

    }
    private void ApprovalRequestInsert()
    {
        PhoenixCommonApproval.ApprovalRequestInsert(new Guid(arrValues[1]), General.GetNullableString(arrValues[2]), General.GetNullableString(arrValues[3]));
    }
    private void SetCurrentDMSDocumentSelection()
    {
        Filter.CurrentDMSDocumentSelection   = "";
        string args = "";

        if (arrNames.Length > 2)
        {
            Filter.CurrentDMSDocumentSelection = arrValues[1];
            if (arrValues[2] == "6")
            {
                DataSet ds = PhoenixDocumentManagementForm.FormEdit(
                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                  , new Guid(arrValues[1]));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (dr["FLDFORMREVISIONDTKEY"] != null && General.GetNullableGuid(dr["FLDFORMREVISIONDTKEY"].ToString()) != null)
                    {
                        DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(dr["FLDFORMREVISIONDTKEY"].ToString()));
                        if (dt.Rows.Count > 0)
                            args += "path=" + Session["sitepath"] + "/attachments/" + dt.Rows[0]["FLDFILEPATH"].ToString();
                    }
                }
            }
        }
        Response.Write(args);
    }
    private void SetCurrentDMSPickListSelection()
    {
        Filter.CurrentDMSDocumentFilter = null;

        if (arrNames.Length > 2)
        {
            NameValueCollection nvc;
            nvc = Filter.CurrentPickListSelection;

            nvc.Set(nvc.GetKey(1), arrValues[2]);
            nvc.Set(nvc.GetKey(2), arrValues[1]);

            Filter.CurrentPickListSelection = nvc;
        }
    }
    private void PopulateDMSTreeOnDemand()
    {
        //Filter.CurrentDMSDocumentSelection = "";
        string args = "";

        if (arrNames.Length > 2)
        {
            if (arrValues[2] == "1")
            {
                DataSet ds = PhoenixDocumentManagementDocument.OfficeHSEQATreeNodeEdit(
                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                  , new Guid(arrValues[1])
                                 , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    args += dr["FLDHTML"].ToString();
                }
            }

            if (arrValues[2] == "2")
            {
                DataSet ds = PhoenixDocumentManagementDocument.DocumentHTMLEdit(
                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                  , new Guid(arrValues[1])
                                  , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    args += dr["FLDHTML"].ToString();
                }
            }
        }
        Response.Write(args);
    }

    private void PopulateNewDMSTreeOnDemand()
    {
        //Filter.CurrentDMSDocumentSelection = "";
        string args = "";

        if (arrNames.Length > 2)
        {
            if (arrValues[2] == "1")
            {
                DataSet ds = PhoenixDocumentManagementDocument.OfficeHSEQATreeNodeEdit(
                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                  , new Guid(arrValues[1])
                                 , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    args += dr["FLDNEWHTML"].ToString();
                }
            }

            if (arrValues[2] == "2")
            {
                DataSet ds = PhoenixDocumentManagementDocument.DocumentHTMLEdit(
                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                  , new Guid(arrValues[1])
                                  , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    args += dr["FLDNEWHTML"].ToString();
                }
            }
        }
        Response.Write(args);
    }

    private void SaveTankPlanLoadConsumption()
    {
        try
        {
            NameValueCollection nvc = new NameValueCollection();
            string dateloaded = "", robmt = "", robcum = "", configurationid = "", vessel = "", midnightreportid = "", tankplanloadconsumptionid = "", tankcleaned="";
            for (int i = 0; i < arrNames.Length; i++)
            {
                nvc.Add(arrNames[i], arrValues[i]);
            }


            vessel = nvc.Get("vessel").ToString();
            midnightreportid = nvc.Get("midnightreportid").ToString();
            tankplanloadconsumptionid = nvc.Get("tankplanloadconsumptionid").ToString();
            configurationid = nvc.Get("configurationid").ToString();
            dateloaded = nvc.Get("dateloaded").ToString();
            robmt = nvc.Get("robmt").ToString();
            robcum = nvc.Get("robcum").ToString();
            tankcleaned = nvc.Get("tankcleaned").ToString();
            if (tankcleaned == "true")
            {
                tankcleaned = "1";
            }
            else
            {
                tankcleaned = "0";
            }

            PhoenixCrewOffshoreDMRMidNightReportTankPlan.CrewOffshoreMidNightReportTankPlanLoadandConsumptionUpdate(int.Parse(vessel),
                                                                                                                    General.GetNullableGuid(midnightreportid),
                                                                                                                    General.GetNullableDateTime(dateloaded),
                                                                                                                    General.GetNullableDecimal(robmt),
                                                                                                                    General.GetNullableDecimal(robcum),
                                                                                                                    General.GetNullableGuid(tankplanloadconsumptionid),
                                                                                                                    General.GetNullableGuid(configurationid),
                                                                                                                    int.Parse(tankcleaned));





            Response.Write("Tank PLan Consumption updated successfully.");
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    private void VaidateTab()
    {
        var tabid = arrValues[1].ToString();
        if (string.IsNullOrEmpty(tabid))
        {
            Session["tab"] = DateTime.Now.Ticks.ToString("x");
            Response.Write("tab_" + Session["tab"].ToString());
        }
        else if (Session["tab"] != null && "tab_" + Session["tab"].ToString() != tabid)
        {
            Response.Write("PhoenixBrowsingRestriction.aspx");
        }
    }

    private void EmployeeListJson()
    {
        Response.AddHeader("Content-Type", "application/json");
        var parameter = arrValues[1].ToString();
        string value = PhoenixFormBuilder.FetchValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, "EMPLOYEELIST", parameter);
        Response.Write(value);
    }
    private void CrewListJson()
    {
        Response.AddHeader("Content-Type", "application/json");
        var parameter = arrValues[1].ToString();
        var reportid = arrValues[2].ToString();
        int VesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        if (!reportid.Equals(""))
        {
            DataTable dt = PhoenixFormBuilder.ReportEdit(new Guid(reportid));
            if (dt.Rows.Count > 0)
            {
                VesselId = int.Parse(dt.Rows[0]["FLDVESSELID"].ToString());
            }
        }
        string value = PhoenixFormBuilder.FetchValue(VesselId, "CREWLIST", parameter);
        Response.Write(value);
    }
    private void FBValueFetch()
    {
        Response.AddHeader("Content-Type", "application/json");
        var data = arrValues[1].ToString();
        var parameter = arrValues[2].ToString();
        var reportid = arrValues[3].ToString();
        JObject fbjson = JObject.Parse(data);
        string[] keys = { "company", "vessel", "user", "revisionNo", "imo", "type", "flag", "owner", "class", "delivery", "portOfRegistry", "callSign", "grossTonnage" };
        foreach (string key in keys)
        {
            foreach (var token in fbjson.SelectTokens("*").Where(s => s.Path.Contains(key)))
            {
                if (key.ToUpper() == "VESSEL" && reportid.Equals(""))
                {
                    var prop = token.Parent as JProperty;
                    if (prop != null) prop.Value = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                }
                if (key.ToUpper() == "COMPANY" && reportid.Equals(""))
                {
                    var prop = token.Parent as JProperty;
                    DataRow dr = null;
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    {
                        DataTable dt = PhoenixFormBuilder.VesselCompany(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                        if (dt.Rows.Count > 0)
                            dr = dt.Rows[0];
                    }
                    else
                    {
                        DataSet ds = PhoenixFormBuilder.CompanyList(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            dr = ds.Tables[0].Rows[0];
                    }
                    if (dr != null)
                    {
                        string value = dr["FLDCOMPANYNAME"].ToString();
                        if (prop != null) prop.Value = value;
                    }

                }

                else if (key.ToUpper() == "REVISIONNO" && reportid.Equals(""))
                {
                    string param = string.Empty;
                    var prop = token.Parent as JProperty;
                    foreach (string s in parameter.Split('`'))
                    {
                        if (s.Contains(prop.Name + "~"))
                        {
                            param = s.Substring(s.IndexOf('~') + 1);
                        }
                    }
                    string value = PhoenixFormBuilder.FetchValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, key, param);
                    if (prop != null) prop.Value = value;
                }
                else if (key.ToUpper() == "CALLSIGN")
                {
                    string param = string.Empty;
                    var prop = token.Parent as JProperty;
                    foreach (string s in parameter.Split('`'))
                    {
                        if (s.Contains(prop.Name + "~"))
                        {
                            param = s.Substring(s.IndexOf('~') + 1);
                        }
                    }
                    string value = PhoenixFormBuilder.FetchValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, key, param);
                    if (prop != null) prop.Value = value;
                }
                else if (key.ToUpper() == "FLAG")
                {
                    string param = string.Empty;
                    var prop = token.Parent as JProperty;
                    foreach (string s in parameter.Split('`'))
                    {
                        if (s.Contains(prop.Name + "~"))
                        {
                            param = s.Substring(s.IndexOf('~') + 1);
                        }
                    }
                    string value = PhoenixFormBuilder.FetchValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, key, param);
                    if (prop != null) prop.Value = value;
                }


                else if (key.ToUpper() == "OWNER")
                {
                    string param = string.Empty;
                    var prop = token.Parent as JProperty;
                    foreach (string s in parameter.Split('`'))
                    {
                        if (s.Contains(prop.Name + "~"))
                        {
                            param = s.Substring(s.IndexOf('~') + 1);
                        }
                    }
                    string value = PhoenixFormBuilder.FetchValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, key, param);
                    if (prop != null) prop.Value = value;
                }
                else if (key.ToUpper() == "CLASS")
                {
                    string param = string.Empty;
                    var prop = token.Parent as JProperty;
                    foreach (string s in parameter.Split('`'))
                    {
                        if (s.Contains(prop.Name + "~"))
                        {
                            param = s.Substring(s.IndexOf('~') + 1);
                        }
                    }
                    string value = PhoenixFormBuilder.FetchValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, key, param);
                    if (prop != null) prop.Value = value;
                }
                else if (key.ToUpper() == "TYPE")
                {
                    string param = string.Empty;
                    var prop = token.Parent as JProperty;
                    foreach (string s in parameter.Split('`'))
                    {
                        if (s.Contains(prop.Name + "~"))
                        {
                            param = s.Substring(s.IndexOf('~') + 1);
                        }
                    }
                    string value = PhoenixFormBuilder.FetchValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, key, param);
                    if (prop != null) prop.Value = value;
                }
                else if (key.ToUpper() == "IMO")
                {
                    string param = string.Empty;
                    var prop = token.Parent as JProperty;
                    foreach (string s in parameter.Split('`'))
                    {
                        if (s.Contains(prop.Name + "~"))
                        {
                            param = s.Substring(s.IndexOf('~') + 1);
                        }
                    }
                    string value = PhoenixFormBuilder.FetchValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, key, param);
                    if (prop != null) prop.Value = value;
                }
                else if (key.ToUpper() == "DELIVERY")
                {
                    string param = string.Empty;
                    var prop = token.Parent as JProperty;
                    foreach (string s in parameter.Split('`'))
                    {
                        if (s.Contains(prop.Name + "~"))
                        {
                            param = s.Substring(s.IndexOf('~') + 1);
                        }
                    }
                    string value = PhoenixFormBuilder.FetchValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, key, param);
                    if (prop != null) prop.Value = value;
                }
                else if (key.ToUpper() == "PORTOFREGISTRY")
                {
                    string param = string.Empty;
                    var prop = token.Parent as JProperty;
                    foreach (string s in parameter.Split('`'))
                    {
                        if (s.Contains(prop.Name + "~"))
                        {
                            param = s.Substring(s.IndexOf('~') + 1);
                        }
                    }
                    string value = PhoenixFormBuilder.FetchValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, key, param);
                    if (prop != null) prop.Value = value;
                }
                else if (key.ToUpper() == "USER")
                {
                    string param = string.Empty;
                    var prop = token.Parent as JProperty;
                    if (prop.Name.ToLower().Contains("username")) continue;
                    foreach (string s in parameter.Split('`'))
                    {
                        if (s.Contains(prop.Name + "~"))
                        {
                            param = s.Substring(s.IndexOf('~') + 1);
                        }
                    }
                    string value = PhoenixFormBuilder.FetchValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, key, param);
                    if (prop != null) prop.Value = value;
                }
                else if (key.ToUpper() == "GROSSTONNAGE")
                {
                    string param = string.Empty;
                    var prop = token.Parent as JProperty;
                    foreach (string s in parameter.Split('`'))
                    {
                        if (s.Contains(prop.Name + "~"))
                        {
                            param = s.Substring(s.IndexOf('~') + 1);
                        }
                    }
                    string value = PhoenixFormBuilder.FetchValue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, key, param);
                    if (prop != null) prop.Value = value;
                }


            }
        }
        Response.Write(fbjson.ToString());
    }
    private void ApprovalUpdate()
    {
        var reportid = arrValues[2].ToString();
        var status = arrValues[1].ToString();

        PhoenixFormBuilder.ApprovalStatusUpdate(new Guid(reportid),status);
    }
    private void PrintFB()
    {
        var reportid = arrValues[2].ToString();
        var status = arrValues[1].ToString();
        PhoenixFormBuilder.ReportStatusUpdate(new Guid(reportid), status);
    }
    private void SaveGriFilter()
    {
        var gridid = arrValues[1];
        var page = arrValues[2].ToString();
        var cols = arrValues[3].ToString();
        SessionUtil.UserGridColumnInsert(page, gridid, cols);
    }
</script>

