import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;

import MetodosSql.MetodosSql;
 
/**
 *
 * @author sqlitetutorial.net
 */
public class ConnectionSql {
    
    public static void main(String[] args) {
    	MetodosSql metodos=new MetodosSql();
    	ArrayList<String>lista=metodos.consultarUnaColumna("select id_contrato from contratos where provider_id='NO-IP';");
    	
    	for(int i=0;i<lista.size();i++) {
    	configurarDVR(Integer.parseInt(lista.get(i)));	 
    	}
    
    	
       } 
    
    
    
    
    
    
    
    
    
    private static void configurarDVR(int id_contrato) {
    	MetodosSql metodos=new MetodosSql();
    	String fechaHoy=null;
    	int errorCode=-1;
    	Process process; 
      	String sentenciaRuntime=crearSentenciaEjecutable(id_contrato);	
    	
    	    	
    	try {
    		if(sentenciaRuntime!=null) {
			 process=Runtime.getRuntime().exec(sentenciaRuntime);
			 InputStreamReader input = new InputStreamReader(process.getInputStream());
	         BufferedReader stdInput = new BufferedReader(input);
	         
	         String aux=stdInput.readLine();
	         while(aux != null)
	         {
	        	System.out.println(aux);
	        	aux=stdInput.readLine();
	        
	         }
	         errorCode=process.exitValue();
	         System.out.println("Codigo de error-> "+errorCode);
	         fechaHoy=metodos.dameFechaDeHoyConFormatoX("YYYY/MM/dd hh:mm:ss");
	         metodos.hacerUpdate("update contratos set ultimoerrorcode="+errorCode+",fechaproceso='"+fechaHoy+"' where id_contrato="+id_contrato+";");
    		}else {
    			System.out.println("Input incorrecto, no existe el id_contrato ="+id_contrato+" se bueno :)");
    		}
		} catch (IOException e) {
			System.out.println(e.getMessage());
		}
    }

	private static String crearSentenciaEjecutable(int contrato_id) {
		MetodosSql metodos=new MetodosSql();
		String sentenciaEjecutar=null;
		String ejecutableHickVision=metodos.consultarUnaCelda("SELECT exename FROM Masvideotype where masvideotype_id='HIK';");
		String ejecutableTVT=metodos.consultarUnaCelda("SELECT exename FROM Masvideotype where masvideotype_id='TVT';");
		String queryTVT=""
				+ "select co.dvrusrlogin,co.dvrpasswlogin,co.dvripourl,co.dvrsdkport,co.dvrnewurl,co.dvrdnslogin,co.dvrdnspass,i.index_id from contratos co \r\n" + 
				"inner join Indexprovider i\r\n" + 
				"where i.provider_id=co.provider_id and i.masvideotype_id=co.masvideotype_id and co.id_contrato= ";
		
		String queryHickvision="\r\n" + 
				"select co.dvrusrlogin,co.dvrpasswlogin,co.dvripourl,co.dvrsdkport,co.dvrnewurl,co.dvrdnslogin,co.dvrdnspass,i.index_id,p.dnshost from contratos co \r\n" + 
				"inner join Indexprovider i\r\n" + 
				"inner join Provider p\r\n" + 
				"where i.provider_id=co.provider_id \r\n" + 
				"and i.masvideotype_id=co.masvideotype_id \r\n" + 
				"and  p.provider_id=co.provider_id \r\n" + 
				"and co.id_contrato= ";
		
		String proveedor=definirProveedor(contrato_id);
		
		if(proveedor.equalsIgnoreCase("HIK")) {
			System.out.println("HickVision detectado, usar ejecutable "+ejecutableHickVision);
			sentenciaEjecutar=metodos.consultarLinea(queryHickvision+contrato_id);
			sentenciaEjecutar=sentenciaEjecutar.replaceAll("Ejecutable", ejecutableHickVision);
			
		}else 
			if (proveedor.equalsIgnoreCase("TVT")) {
			System.out.println("TVT detectado, usar ejecutable "+ejecutableTVT);
			sentenciaEjecutar=metodos.consultarLinea(queryTVT+contrato_id);
			sentenciaEjecutar=sentenciaEjecutar.replaceAll("Ejecutable", ejecutableTVT);
			
			}	
		
		System.out.println(sentenciaEjecutar);
		return sentenciaEjecutar;
	}
	
	
	private static String definirProveedor(int contrato_Id) {
		 MetodosSql metodos=new MetodosSql();
		 return metodos.consultarUnaCelda("select masvideotype_id from contratos where id_contrato= "+contrato_Id);
		
	}
}