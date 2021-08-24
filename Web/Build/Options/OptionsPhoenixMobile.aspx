<%@ Page Language="C#" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>


<script language="c#" runat="server">
    
    private string strFunctionName;
    private string[] arrNames;
    private string[] arrValues;

    private void GetParameters(string strQueryString)
    {
        string[] arrURLParameters;


        arrURLParameters = strQueryString.Split('&');
        arrNames = new string[arrURLParameters.Length];
        arrValues = new string[arrURLParameters.Length];

        for (int i = 0; i < arrURLParameters.Length; i++)
        {
            arrNames[i] = arrURLParameters[i].Split('=')[0];
            arrValues[i] = arrURLParameters[i].Split('=')[1];
        }

        strFunctionName = arrValues[0];
    }

    
    protected void Page_Load(object o, EventArgs e)
    {
        Session.Timeout = 120;
        GetParameters(Server.UrlDecode((new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd()));

        if (strFunctionName.ToUpper().Equals("VESSELLIST"))
        {
            Response.Write(GetVesselList());
        }
        if (strFunctionName.ToUpper().Equals("VESSELLISTBYFLEET"))
        {
            Response.Write(GetVesselListByFleet());
        }        
        if (strFunctionName.ToUpper().Equals("CREWLIST"))
        {
            Response.Write(GetCrewList());
        }
        if (strFunctionName.ToUpper().Equals("CREW"))
        {
            Response.Write(GetCrew());
        }
        if (strFunctionName.ToUpper().Equals("VESSEL"))
        {
            Response.Write(GetVessel());
        }
        if (strFunctionName.ToUpper().Equals("AUDITINSPECTIONLIST"))
        {
            Response.Write(GetAuditInspectionList());
        }
        if (strFunctionName.ToUpper().Equals("AUDITINSPECTION"))
        {
            Response.Write(GetAuditInspection());
        }        
        if (strFunctionName.ToUpper().Equals("VETTINGLIST"))
        {
            Response.Write(GetVettingList());
        }
        if (strFunctionName.ToUpper().Equals("VETTING"))
        {
            Response.Write(GetVetting());
        }        
        if (strFunctionName.ToUpper().Equals("CERTIFICATESDUEIN60DAYS"))
        {
            Response.Write(GetCertificatesDueIn60Days());
        }
        if (strFunctionName.ToUpper().Equals("MAINTENANCELIST"))
        {
            Response.Write(GetMaintenanceList());
        }
        if (strFunctionName.ToUpper().Equals("FORMLIST"))
        {
            Response.Write(GetFormList());
        }
        if (strFunctionName.ToUpper().Equals("FORM"))
        {
            Response.Write(GetForm());
        }
        if (strFunctionName.ToUpper().Equals("FORMITEMLIST"))
        {
            Response.Write(GetFormItems());
        }           
        if (strFunctionName.ToUpper().Equals("QUOTATIONLIST"))
        {
            Response.Write(GetQuotationList());
        }
        if (strFunctionName.ToUpper().Equals("QUOTATION"))
        {
            Response.Write(GetQuotation());
        }        
        if (strFunctionName.ToUpper().Equals("QUOTATIONITEMS"))
        {
            Response.Write(GetQuotationItems());
        }
        if (strFunctionName.ToUpper().Equals("CREWCHANGEPLAN"))
        {
            Response.Write(GetCrewChangePlan());
        }
        if (strFunctionName.ToUpper().Equals("COMPONENTLIST"))
        {
            Response.Write(GetComponentList());
        }
        if (strFunctionName.ToUpper().Equals("CHILDCOMPONENTLIST"))
        {
            Response.Write(GetChildComponentList());
        }
        if (strFunctionName.ToUpper().Equals("COMPONENT"))
        {
            Response.Write(GetComponent());
        }                  
        if (strFunctionName.ToUpper().Equals("COMPONENTTREE"))
        {
            Response.Write(GetComponentTree());
        }
        if (strFunctionName.ToUpper().Equals("SPAREITEMLIST"))
        {
            Response.Write(GetSpareItemList());
        }
        if (strFunctionName.ToUpper().Equals("FLEETLIST"))
        {
            Response.Write(GetFleetList());
        }                                             
        if (strFunctionName.ToUpper().Equals("USERLOGIN"))
        {
            Response.Write(UserLogin());
        }          
    }

    private string GetVesselList()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        string vesselname = arrValues[1];
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELNAME", SqlDbType.VarChar, DbConstant.VARCHAR_200, ParameterDirection.Input, vesselname));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEVESSELLIST", ParameterList);

        return ConvertDataTable2JSON(dt);
    }

    private string GetVesselListByFleet()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int fleetid = int.Parse(arrValues[1]);
        ParameterList.Add(DataAccess.GetDBParameter("@FLEETID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, fleetid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEVESSELLISTBYFLEET", ParameterList);

        return ConvertDataTable2JSON(dt);
    }    

    private string GetCrewList()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILECREWLIST", ParameterList);

        return ConvertDataTable2JSON(dt);
    }

    private string GetCrew()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int employeeid = int.Parse(arrValues[1]);
        ParameterList.Add(DataAccess.GetDBParameter("@EMPLOYEEID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, employeeid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILECREW", ParameterList);

        return ConvertDataTable2JSON(dt);
    }

    private string GetVessel()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEVESSEL", ParameterList);

        return ConvertDataTable2JSON(dt);
    }

    private string GetAuditInspectionList()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEAUDITINSPECTIONSCHEDULE", ParameterList);

        return ConvertDataTable2JSON(dt);
    }

    private string GetAuditInspection()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        Guid inspectionid = new Guid(arrValues[2]);
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        ParameterList.Add(DataAccess.GetDBParameter("@INSPECTIONID", SqlDbType.UniqueIdentifier, DbConstant.UNIQUEIDENTIFIER, ParameterDirection.Input, inspectionid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEAUDITINSPECTIONSCHEDULE", ParameterList);

        return ConvertDataTable2JSON(dt);
    }    

    private string GetVettingList()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEVETTINGSCHEDULE", ParameterList);

        return ConvertDataTable2JSON(dt);
    }

    private string GetVetting()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        Guid inspectionid = new Guid(arrValues[2]);
        
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        ParameterList.Add(DataAccess.GetDBParameter("@INSPECTIONID", SqlDbType.UniqueIdentifier, DbConstant.UNIQUEIDENTIFIER, ParameterDirection.Input, inspectionid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEVETTINGSCHEDULE", ParameterList);

        return ConvertDataTable2JSON(dt);
    }    

    private string GetCertificatesDueIn60Days()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEALERTCERITIFICATESDUEIN60DAYS", ParameterList);

        return ConvertDataTable2JSON(dt);
    }

    private string GetMaintenanceList()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        string option = arrValues[2];
        
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        ParameterList.Add(DataAccess.GetDBParameter("@OPTION", SqlDbType.VarChar, DbConstant.VARCHAR_20, ParameterDirection.Input, option));   
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEMAINTENANCEDUE", ParameterList);

        return ConvertDataTable2JSON(dt);
    }

    private string GetFormList()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        string stocktype = arrValues[2];

        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        ParameterList.Add(DataAccess.GetDBParameter("@STOCKTYPE", SqlDbType.VarChar, DbConstant.VARCHAR_20, ParameterDirection.Input, stocktype));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEFORMLIST", ParameterList);

        return ConvertDataTable2JSON(dt);
    }

    private string GetForm()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        string orderid = arrValues[2];

        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        ParameterList.Add(DataAccess.GetDBParameter("@ORDERID", SqlDbType.VarChar, DbConstant.VARCHAR_50, ParameterDirection.Input, orderid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEFORM", ParameterList);

        return ConvertDataTable2JSON(dt);
    }

    private string GetFormItems()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        string orderid = arrValues[2];

        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        ParameterList.Add(DataAccess.GetDBParameter("@ORDERID", SqlDbType.VarChar, DbConstant.VARCHAR_50, ParameterDirection.Input, orderid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEORDERLINE", ParameterList);

        return ConvertDataTable2JSON(dt);
    }



    private string GetComponentList()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);

        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILECOMPONENTCLASS", ParameterList);

        return ConvertDataTable2JSON(dt);
    }


    private string GetChildComponentList()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        string componentid = arrValues[2];

        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        ParameterList.Add(DataAccess.GetDBParameter("@COMPONENTID", SqlDbType.VarChar, DbConstant.VARCHAR_50, ParameterDirection.Input, componentid));

        dt = DataAccess.ExecSPReturnDataTable("PRMOBILECHILDCOMPONENTS", ParameterList);

        return ConvertDataTable2JSON(dt);
    }

    private string GetComponent()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        string componentid = arrValues[2];

        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        ParameterList.Add(DataAccess.GetDBParameter("@COMPONENTID", SqlDbType.VarChar, DbConstant.VARCHAR_50, ParameterDirection.Input, componentid));

        dt = DataAccess.ExecSPReturnDataTable("PRMOBILECOMPONENT", ParameterList);

        return ConvertDataTable2JSON(dt);
    }     
    
    private string GetComponentTree()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        string componentid = arrValues[2];
        
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        ParameterList.Add(DataAccess.GetDBParameter("@COMPONENTID", SqlDbType.VarChar, DbConstant.VARCHAR_50, ParameterDirection.Input, componentid));
        
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILECOMPONENTTREE", ParameterList);

        return ConvertDataTable2JSON(dt);
    }

    private string GetSpareItemList()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        string componentid = arrValues[2];

        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        ParameterList.Add(DataAccess.GetDBParameter("@COMPONENTID", SqlDbType.VarChar, DbConstant.VARCHAR_50, ParameterDirection.Input, componentid));

        dt = DataAccess.ExecSPReturnDataTable("PRMOBILESPAREITEMLIST", ParameterList);

        return ConvertDataTable2JSON(dt);
    }    
            
    private string GetQuotationList()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        string orderid = arrValues[2];

        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        ParameterList.Add(DataAccess.GetDBParameter("@ORDERID", SqlDbType.VarChar, DbConstant.VARCHAR_50, ParameterDirection.Input, orderid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEQUOTATIONLIST", ParameterList);

        return ConvertDataTable2JSON(dt);
    }

    private string GetQuotation()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        string orderid = arrValues[2];
        string quotationid = arrValues[3];

        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        ParameterList.Add(DataAccess.GetDBParameter("@ORDERID", SqlDbType.VarChar, DbConstant.VARCHAR_50, ParameterDirection.Input, orderid));
        ParameterList.Add(DataAccess.GetDBParameter("@QUOTATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_50, ParameterDirection.Input, quotationid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEQUOTATION", ParameterList);

        return ConvertDataTable2JSON(dt);
    }    
    
    private string GetQuotationItems()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        string orderid = arrValues[2];
        string quotationid = arrValues[3];

        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        ParameterList.Add(DataAccess.GetDBParameter("@ORDERID", SqlDbType.VarChar, DbConstant.VARCHAR_50, ParameterDirection.Input, orderid));
        ParameterList.Add(DataAccess.GetDBParameter("@QUOTATIONID", SqlDbType.VarChar, DbConstant.VARCHAR_50, ParameterDirection.Input, quotationid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEQUOTATIONITEMS", ParameterList);

        return ConvertDataTable2JSON(dt);
    }


    private string GetCrewChangePlan()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        int vesselid = int.Parse(arrValues[1]);
        
        ParameterList.Add(DataAccess.GetDBParameter("@VESSELID", SqlDbType.Int, DbConstant.INT, ParameterDirection.Input, vesselid));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILECREWCHANGEPLAN", ParameterList);

        return ConvertDataTable2JSON(dt);
    }

    private string GetFleetList()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        dt = DataAccess.ExecSPReturnDataTable("PRMOBILEFLEETLIST", ParameterList);

        return ConvertDataTable2JSON(dt);
    }                       
    public string UserLogin()
    {
        DataTable dt = new DataTable();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        string username = arrValues[1];
        string password = arrValues[2];

        ParameterList.Add(DataAccess.GetDBParameter("@USERNAME", SqlDbType.VarChar, DbConstant.NVARCHAR_200, ParameterDirection.Input, username));
        ParameterList.Add(DataAccess.GetDBParameter("@PASSWORD", SqlDbType.VarChar, DbConstant.NVARCHAR_200, ParameterDirection.Input, password));
        dt = DataAccess.ExecSPReturnDataTable("PRMOBILELOGIN", ParameterList);

        return ConvertDataTable2JSON(dt);    
    }
    
    public string ConvertDataTable2JSON(DataTable dt)
    {        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                row.Add(col.ColumnName, dr[col]);
            }
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }    
    
</script>

