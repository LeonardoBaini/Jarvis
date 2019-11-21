#include <stdio.h>
#include <iostream>
#include "Windows.h"
#include "HCNetSDK.h"
using namespace std;

int main(int argc, char **argv) {

    // VER EL TEMA DE LONGITUD
    char ipOriginal[MAX_DOMAIN_NAME]={0};
	char userOriginal[NAME_LEN]={0};
	char passwordOriginal[PASSWD_LEN]={0};
    char ipDDNSnuevo[MAX_DOMAIN_NAME]={0};
	char userDDNSnuevo[NAME_LEN]={0};
	char passwordDDNSnuevo[PASSWD_LEN]={0};
	char serverDDNSnuevo[MAX_DOMAIN_NAME]={0};
	int  tipoDDNS=0;
	int  portSdk=0;


    if(argc<9){
    cout << "La ejecucion requiere 8 parametros, ingrese:\n";
    cout << "HickVisionConfigurer.exe Param1 Param2 Param3 Param4 Param5 Param6 Param7 Param8 Param9\n";
    cout << "Donde\n";
    cout << "Param 1 = Usuario de login al DVR\n";
    cout << "Param 2 = Contrasena de login al DVR\n";
    cout << "Param 3 = URL o IP del equipo\n";
    cout << "Param 4 = Puerto SDK\n";
    cout << "Param 5 = Nueva URL\n";
    cout << "Param 6 = Nuevo usuario\n";
    cout << "Param 7 = Nueva contrasena\n";
    cout << "Param 8 = DYNDNS=1 / NO-IP=3\n";
    cout << "Param 9 = Servidor nuevo\n";

    /*Reservado para test*/
    strcpy(ipDDNSnuevo,"hiwatch03.es.camaras.proseguralarmas.com");
    strcpy(userDDNSnuevo,"155gSC-prosegur");
    strcpy(passwordDDNSnuevo,"prosegur");
    tipoDDNS=1;
    strcpy(userOriginal, "admin");
    strcpy(passwordOriginal,"87654321z");
    strcpy(ipOriginal,"hiwatch03.es.camaras.proseguralarmas.com");
    portSdk =6037;
    tipoDDNS=3;


    /*
    ipDDNSnuevo="pruebahickvision.ddns.net";
    userDDNSnuevo="bainileonardo@gmail.com";
    passwordDDNSnuevo="Patagon1an";
    tipoDDNS=3;
    */

    /*
    strcpy(userOriginal, "admin");
    strcpy(passwordOriginal,"87654321z");
    strcpy(ipOriginal,"hiwatch03.es.camaras.proseguralarmas.com");
    portSdk =6037;
    tipoDDNS=3;
    */

    //return 2000;

    }
    else{
    strcpy(userOriginal, argv[1]);
    strcpy(passwordOriginal, argv[2]);
    strcpy(ipOriginal, argv[3]);
    portSdk =atoi(argv[4]);
    strcpy(ipDDNSnuevo, argv[5]);
    strcpy(userDDNSnuevo, argv[6]);
    strcpy(passwordDDNSnuevo, argv[7]);
    tipoDDNS=atoi(argv[8]);
    strcpy(serverDDNSnuevo, argv[9]);


    cout <<"userOriginal:"<<userOriginal<<"\n";
    cout <<"passwordOriginal:"<<passwordOriginal<<"\n";
    cout <<"ipOriginal:"<<ipOriginal<<"\n";
    cout <<"portSdk:"<<portSdk<<"\n";
    cout <<"ipDDNSnuevo:"<<ipDDNSnuevo<<"\n";
    cout <<"userDDNSnuevo:"<<userDDNSnuevo<<"\n";
    cout <<"passwordDDNSnuevo:"<<passwordDDNSnuevo<<"\n";
    cout <<"tipoDDNS:"<<tipoDDNS<<"\n";
    cout <<"serverDDNSnuevo:"<<serverDDNSnuevo<<"\n";
    cout << endl;



    }
	//---------------------------------------
	//Init device
	NET_DVR_Init();
	//Set connect time and reconnect time
	NET_DVR_SetConnectTime(10000, 1);
	NET_DVR_SetReconnect(10000, true);

	//---------------------------------------
	// Register device
	LONG lUserID;
	NET_DVR_DEVICEINFO_V30 struDeviceInfo;

	lUserID = NET_DVR_Login_V30(ipOriginal, portSdk, userOriginal, passwordOriginal, &struDeviceInfo);

	printf("Este es el serial number-->  ");
	printf((char*)&struDeviceInfo.sSerialNumber);

	printf("\n");


	// Hasta acá logueado.
	//******************************************************************************************************************

	if (lUserID < 0)
  {
       printf("Login error, %d\n", (int)NET_DVR_GetLastError());
       NET_DVR_Cleanup();
       return NET_DVR_GetLastError();
  }

  int iRet;
  //Get compression parameter
  DWORD dwreturn;
  NET_DVR_DDNSPARA_V30 struParams2 = {0};

  iRet = NET_DVR_GetDVRConfig(lUserID, NET_DVR_GET_DDNSCFG_V30, struDeviceInfo.byStartChan,
          &struParams2, sizeof(NET_DVR_DDNSPARA_V30), &dwreturn);

        //  printf("Video Bitrate is %d\n", struParams.struNormHighRecordPara.dwVideoBitrate);
        printf((char*)struParams2.struDDNS[tipoDDNS].sUsername);
        printf("\n");
        printf((char*)struParams2.struDDNS[tipoDDNS].sPassword);
        printf("\n");
        printf((char*)struParams2.struDDNS[tipoDDNS].sServerName);
        printf("\n");
        printf((char*)struParams2.struDDNS[tipoDDNS].sDomainName);

        printf("\n");



  if (!iRet)
  {
     // printf("NET_DVR_GetDVRConfig NET_DVR_GET_DDNSCFG_V30 error.\n");
      printf((char*)NET_DVR_GetErrorMsg());
      NET_DVR_Logout_V30(lUserID);
      NET_DVR_Cleanup();
      return NET_DVR_GetLastError();
  }
/*Inicializar dns posibles para borrar basura*/
int i=0;
while (i < MAX_DDNS_NUMS){
        struParams2.struDDNS[i]={0};
       /* struParams2.struDDNS[i].sUsername={0};
        struParams2.struDDNS[i].sPassword={0};
        struParams2.struDDNS[i].sServerName={0};*/
    i++;
}




/*ASIGNACION DE VALORES AL NUEVO OBJETO*/

struParams2.byHostIndex=tipoDDNS; // no ip =3 Dyndns=1
struParams2.byEnableDDNS=1;

for(int j=0;j<MAX_DOMAIN_NAME;j++){
struParams2.struDDNS[tipoDDNS].sDomainName[j]=ipDDNSnuevo[j];
}

for(int i=0;i<NAME_LEN;i++){
struParams2.struDDNS[tipoDDNS].sUsername[i]=userDDNSnuevo[i];
}
for(int i=0;i<PASSWD_LEN;i++){
struParams2.struDDNS[tipoDDNS].sPassword[i]=passwordDDNSnuevo[i];
}
for(int i=0;i<MAX_DOMAIN_NAME;i++){
struParams2.struDDNS[tipoDDNS].sServerName[i]=serverDDNSnuevo[i];
}







iRet = NET_DVR_SetDVRConfig(lUserID, NET_DVR_SET_DDNSCFG_V30, struDeviceInfo.byStartChan,
          &struParams2, sizeof(NET_DVR_DDNSPARA_V30));

  if (!iRet)
  {
      printf((char*)NET_DVR_GetErrorMsg());
      NET_DVR_Logout_V30(lUserID);
      NET_DVR_Cleanup();
      return NET_DVR_GetLastError();
  }


 iRet = NET_DVR_GetDVRConfig(lUserID, NET_DVR_GET_DDNSCFG_V30, struDeviceInfo.byStartChan,
          &struParams2, sizeof(NET_DVR_DDNSPARA_V30), &dwreturn);

  if (!iRet)
  {
     printf((char*)NET_DVR_GetErrorMsg());
      NET_DVR_Logout_V30(lUserID);
      NET_DVR_Cleanup();
      return NET_DVR_GetLastError();
  }



  NET_DVR_RebootDVR(lUserID);
  //Logout device
  NET_DVR_Logout(lUserID);
  //Release sdk resource
  NET_DVR_Cleanup();


  return 1;
}



