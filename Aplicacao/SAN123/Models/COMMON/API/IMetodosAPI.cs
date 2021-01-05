using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Models.COMMON.API
{
    public interface IMetodosAPI
    {
        MollaLibrary.COMMON.RetornoRequisicao MTD_Autenticacao(string pLogin, string pSenha, out bool pAcessoFake);
        
        MollaLibrary.COMMON.RetornoRequisicao MTD_AlterarSenha(string pNovaSenha);
        MollaLibrary.COMMON.RetornoRequisicao MTD_CRUD(Models.SITE.Login.DadosUsuario cadastrais);

        MollaLibrary.COMMON.RetornoRequisicao MTD_AceiteRegulamento(string pLogin);

     

        dynamic MTD_ObterUsuario(string pLogin);

        dynamic MTD_ListarUsuarios(string pPIT);

        dynamic MTD_ObterUsuarioCrud(string pLogin);


    }
}
