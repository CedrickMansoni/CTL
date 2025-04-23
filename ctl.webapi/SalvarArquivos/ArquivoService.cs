using System;

namespace ctl.webapi.SalvarArquivos;

public class ArquivoService : IArquivoService
{
    public async Task<string> SalvarArquivoAsync(IFormFile arquivo, string storagePath, string pasta)
    {
        if (arquivo == null || arquivo.Length == 0)
        {
            return "Arquivo inválido";
        }

        try
        {
            // Obter a extensão do arquivo
            string extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();

            // Validar extensões permitidas
            if (!ExtensaoPermitida(extensao))
            {
                throw new ArgumentException($"Tipo de arquivo não permitido: {extensao}");
            }

            // Criar o caminho da pasta
            string caminhoUpload = Path.Combine(storagePath, pasta);

            // Certificar que a pasta existe
            if (!Directory.Exists(caminhoUpload))
            {
                Directory.CreateDirectory(caminhoUpload);
            }

            // Criar um nome único para o arquivo            
            string caminhoCompleto = Path.Combine(caminhoUpload, arquivo.FileName);

            // Salvar o arquivo
            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            // Retornar o caminho relativo para armazenamento no banco de dados
            return "Arquivo guardado com sucesso";
        }
        catch
        {
            return "Erro no armazenamento do arquivo";
        }
    }
    private bool ExtensaoPermitida(string extensao)
    {
        // Lista de extensões permitidas
        string[] extensoesPermitidas = { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx" };

        return Array.Exists(extensoesPermitidas, e => e.Equals(extensao, StringComparison.OrdinalIgnoreCase));
    }
}
