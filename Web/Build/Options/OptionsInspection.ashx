<%@ WebHandler Language="C#" Class="OptionsInspection" %>

using System;
using System.Web;
using System.IO;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.Text;
using System.Collections.Generic;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.Common;

public class OptionsInspection : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    HttpRequest request = null;
    HttpResponse response = null;
    HttpServerUtility server = null;

    public void ProcessRequest(HttpContext context)
    {
        request = context.Request;
        response = context.Response;
        server = context.Server;

        string input = "";
        string strFunctionName = request.QueryString["methodname"].ToString();
        string rowusercode = request.QueryString["rowusercode"].ToString();
        input = server.UrlDecode((new System.IO.StreamReader(request.InputStream, Encoding.UTF8)).ReadToEnd()).ToString();

        response.ClearContent();
        response.Buffer = true;
        response.ContentType = "text/xml";

        context.Session["CURRENTDATABASE"] = null;

        if (strFunctionName.ToUpper() == "INSPECTIONVIRDEFICIENCYUPDATE")
        {
            response.Write(InspectionVIRDeficiencyUpdate(input, rowusercode));
        }
        else if (strFunctionName.ToUpper() == "INSPECTIONDEFICIENCYUPDATE")
        {
            response.Write(InspectionDeficiencyUpdate(input, rowusercode));
        }
        else if (strFunctionName.ToUpper() == "INSPECTIONVIRDEFICIENCYCATEGORY")
        {
            response.Write(InspectionVIRDeficiencyCategory(input, rowusercode));
        }
        else if (strFunctionName.ToUpper() == "INSPECTIONCHECKLISTUPDATE")
        {
            response.Write(InspectionCheckListUpdate(input, rowusercode));
        }
        else if (strFunctionName.ToUpper() == "INSPECTIONCHECKLISTDELETE")
        {
            response.Write(InspectionCheckListDelete(input, rowusercode));
        }
    }

    private string InspectionVIRDeficiencyUpdate(string input, string rowusercode)
    {
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixInspectionService.InspectionVIRDeficiencyUpdate(int.Parse(rowusercode), input);

            return "Deficiencies are updated.";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private string InspectionDeficiencyUpdate(string input, string rowusercode)
    {
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixInspectionService.InspectionDeficiencyUpdate(int.Parse(rowusercode), input);

            return "Deficiencies are updated.";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }


    private string InspectionVIRDeficiencyCategory(string input, string rowusercode)
    {
        try
        {

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            string param1 = string.Empty;
            string param2 = string.Empty;
            string param3 = string.Empty;

            XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/RECORDSET/PARAMETER");
            foreach (XmlNode node in nodeList)
            {
                param1 = node.SelectSingleNode("ID").InnerText;
                param2 = node.SelectSingleNode("VALUE").InnerText;
                param3 = node.SelectSingleNode("CODE").InnerText;
            }

            DataSet dsFinal = new DataSet();
            if (param3 == "CATEGORY")
            {
                dsFinal = BindDataCategory(dsFinal, "CATEGORY");
            }
            else if (param3 == "AUDIT_INSPECTION")
            {
                string type = PhoenixCommonRegisters.GetHardCode(1, 148, "AUD");
                int? companyId = 0;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = PhoenixInspectionService.InspectionCompanies(1);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow Row in dt.Rows)
                        {
                            if (Row["FLDSHORTCODE"].ToString() == "ESM SIN")
                            {
                                companyId = (Row["FLDCOMPANYID"] == DBNull.Value ? (int?)null : Convert.ToInt32(Row["FLDCOMPANYID"]));
                                break;
                            }
                        }
                    }
                    if (companyId > 0)
                    {
                        dsFinal = BindDataAuditInspection(dsFinal, "AUDIT", int.Parse(rowusercode), General.GetNullableInteger(type), 710, null, 1, companyId);
                    }
                }
            }
            else
            {
                int? id = 0;
                if (param1 != "")
                    id = Convert.ToInt16(param1);
                else id = (int?)null;
                dsFinal = BindDataChapters(dsFinal, "CHAPTER", int.Parse(rowusercode), new Guid(param2), null, id);

                dsFinal = BindDataChecklist(dsFinal, "LIST", int.Parse(rowusercode), new Guid(param2), id);
            }
            if (dsFinal.Tables.Count < 1)
                return "No Data Available";
            else
                return dsFinal.GetXml();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private string InspectionCheckListUpdate(string input, string rowusercode)
    {
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixInspectionService.InspectionCheckListUpdate(int.Parse(rowusercode), input);

            return "Checklists are updated.";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private string InspectionCheckListDelete(string input, string rowusercode)
    {
        try
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(input);

            PhoenixInspectionService.InspectionCheckListDelete(int.Parse(rowusercode), input);

            return "Checklist(s) are deleted.";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public DataSet BindDataAuditInspection(DataSet dsFinal, string TableName, int rowusercode, int? inspectiontypeid, int? inspectioncategoryid, int? externalaudittype, int? officeaudit, int? companyid)
    {

        DataSet ds = PhoenixInspectionService.InspectionList(rowusercode, inspectiontypeid
                                            , inspectioncategoryid
                                            , externalaudittype
                                            , officeaudit
                                            , companyid);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            ds.Tables[0].TableName = TableName;
            dt = ds.Tables[0].Copy();
            dsFinal.Tables.Add(dt);
        }
        return dsFinal;
    }

    public DataSet BindDataCategory(DataSet dsFinal, string TableName)
    {

        DataSet ds = PhoenixRegistersQuick.ListQuick(1, 47);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            ds.Tables[0].TableName = TableName;
            dt = ds.Tables[0].Copy();
            dsFinal.Tables.Add(dt);
        }
        return dsFinal;
    }

    public DataSet BindDataChapters(DataSet dsFinal, string TableName, int rowusercode, Guid? InspectionId, string shortCode, int? categoryId)
    {

        DataSet ds = PhoenixInspectionService.InspectionChapters(rowusercode, InspectionId, shortCode, categoryId);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            ds.Tables[0].TableName = TableName;
            dt = ds.Tables[0].Copy();
            dsFinal.Tables.Add(dt);
        }
        return dsFinal;
    }

    public DataSet BindDataChecklist(DataSet dsFinal, string TableName, int rowusercode, Guid? InspectionId, int? categoryId)
    {

        DataSet ds = PhoenixInspectionService.GetInspectionCheckLists(rowusercode, InspectionId, categoryId);
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            ds.Tables[0].TableName = TableName;
            dt = ds.Tables[0].Copy();
            dsFinal.Tables.Add(dt);
        }
        return dsFinal;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}