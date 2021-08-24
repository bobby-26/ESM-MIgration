<%@ Page Language="C#" EnableViewState="false" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.IO" %>


<script runat="server">

    private static string XmlFileName = "EPSS_Data.xml";    
	protected void Page_Load(object sender, EventArgs e)	//constructor 
    {                
        string xml = Server.UrlDecode((new StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd());
		CreateEPSSXML(xml);		
    }

    private void CreateEPSSXML(string xml)
    {
        //VALUE FROM FLASH...		
        string cntTxt = xml;
        String[] cntTxtelements = Regex.Split(cntTxt, "~");
        //userName	->	cntTxtelements[1]
        //userID	->	cntTxtelements[2]
        //userRank	->	cntTxtelements[3]
        //courseName	->	cntTxtelements[4]
        //score	->	cntTxtelements[5]
        //ship	->	cntTxtelements[6]
        //examDate	->	cntTxtelements[7]
        //superRank	->	cntTxtelements[8]
        //superName	->	cntTxtelements[9]
        //superID	->	cntTxtelements[10]

        // creating object of XML DOCument class  
        XmlDocument XmlDocObj = new XmlDocument();

        //Select root node which is already defined  	
        XmlNode RootNode = XmlDocObj.AppendChild(XmlDocObj.CreateNode(XmlNodeType.Element, "candidates", ""));

        XmlNode candidateNode = RootNode.AppendChild(XmlDocObj.CreateNode(XmlNodeType.Element, "candidate", ""));

        //--Add attributes to the node "candidate"---//
        XmlAttribute attrName = XmlDocObj.CreateAttribute("userName");
        attrName.Value = cntTxtelements[1];
        candidateNode.Attributes.Append(attrName);

        XmlAttribute attrID = XmlDocObj.CreateAttribute("userID");
        attrID.Value = cntTxtelements[2];
        candidateNode.Attributes.Append(attrID);

        XmlAttribute attrRank = XmlDocObj.CreateAttribute("userRank");
        attrRank.Value = cntTxtelements[3];
        candidateNode.Attributes.Append(attrRank);


        XmlNode courseNode = candidateNode.AppendChild(XmlDocObj.CreateNode(XmlNodeType.Element, "course", ""));
        //--Add attributes to the node "course"---//
        XmlAttribute attrCourseName = XmlDocObj.CreateAttribute("courseName");
        attrCourseName.Value = cntTxtelements[4];
        courseNode.Attributes.Append(attrCourseName);

        courseNode.AppendChild(XmlDocObj.CreateNode(XmlNodeType.Element, "score", "")).InnerText = cntTxtelements[5];
        courseNode.AppendChild(XmlDocObj.CreateNode(XmlNodeType.Element, "duration", "")).InnerText = cntTxtelements[6];
        courseNode.AppendChild(XmlDocObj.CreateNode(XmlNodeType.Element, "attempts", "")).InnerText = cntTxtelements[7];
        courseNode.AppendChild(XmlDocObj.CreateNode(XmlNodeType.Element, "ship", "")).InnerText = cntTxtelements[8];
        courseNode.AppendChild(XmlDocObj.CreateNode(XmlNodeType.Element, "examDate", "")).InnerText = cntTxtelements[9];
        courseNode.AppendChild(XmlDocObj.CreateNode(XmlNodeType.Element, "superRank", "")).InnerText = cntTxtelements[10];
        courseNode.AppendChild(XmlDocObj.CreateNode(XmlNodeType.Element, "superName", "")).InnerText = cntTxtelements[11];
        courseNode.AppendChild(XmlDocObj.CreateNode(XmlNodeType.Element, "superID", "")).InnerText = cntTxtelements[12];

        SouthNests.Phoenix.CrewCommon.PhoenixEPSSImport.InsertEPSSImport(XmlFileName, XmlDocObj.OuterXml);
    }

</script>
