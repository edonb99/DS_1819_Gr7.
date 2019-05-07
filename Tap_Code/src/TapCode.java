import java.util.Scanner;

public class TapCode {
	private static char[][] keyMatrix = { { 'A', 'B', 'C', 'D', 'E' }, 
			                              { 'F', 'G', 'H', 'I', 'J' },
			                              { 'L', 'M', 'N', 'O', 'P' }, 
			                              { 'Q', 'R', 'S', 'T', 'U' }, 
			                              { 'V', 'W', 'X', 'Y', 'Z' } };
	private static String encodedText;
	private static String decodedText;
	private static Scanner scanner;

	public static boolean check(String s) {
		if (s == null) {
			return false;
		}

		for (int i = 0; i < s.length(); i++) {
			if ((Character.isLetter(s.charAt(i)) == false)) {
				return false;
			}
		}

		return true;
	}

	public static void main(String[] args) {

		scanner = new Scanner(System.in);
		boolean continueInput = true;
		while (continueInput) {
			System.out.println("Press E for encode mode, D for decode mode, Q to quit");
			String mode = scanner.nextLine();
			mode = mode.toUpperCase();

			switch (mode) {
			case "E":
				encodeMode();
				break;
			case "D":
				decodeMode();
				break;
			case "Q":
				continueInput = false;
				break;
			default:
				System.out.println("Please write one of the following characters : E,D or Q");
				break;
			}
		}
		System.out.println("Execution finished!");
		scanner.close();

	}

	private static void encodeMode() {

		scanner = new Scanner(System.in);

		boolean continueInput = true;
		while (continueInput) {
			System.out.println("Write the message that you want to encode!");
			decodedText = scanner.nextLine();
			if (!check(decodedText)) {
				System.out.println("Plain text can only have letters! Try again...");
			} else
				continueInput = false;

		}

		decodedText = decodedText.toUpperCase();
		decodedText = decodedText.replace('K', 'C');
		StringBuilder builder = new StringBuilder();
		char[] decodedCharArray = decodedText.toCharArray();
		byte[] encodedByteArray;
		for (int i = 0; i < decodedCharArray.length; i++) {
			char currentChar = decodedCharArray[i];
			if (currentChar == ' ') {
				builder.append("/");
				continue;

			}
			for (int j = 0; j < 5; j++) {
				for (int k = 0; k < 5; k++) {
					if (currentChar == keyMatrix[j][k]) {
						for (int l = 0; l < j + 1; l++) {
							builder.append('.');
						}
						builder.append(' ');
						for (int l = 0; l < k + 1; l++) {
							builder.append('.');
						}
					}
				}
			}
			builder.append(' ');
		}
		encodedText = builder.toString();
		encodedByteArray = new byte[encodedText.length()];
		for (int i = 0; i < encodedText.length(); i++) {
			if (encodedText.charAt(i) == '.') {
				encodedByteArray[i] = 1;
				System.out.print("1");
			} else {
				encodedByteArray[i] = 0;
				System.out.print("0");
			}
		}
		System.out.println();
		System.out.println(encodedText);
	}

	private static void decodeMode() {
		System.out.println("Type the code that you want to decode");
		scanner = new Scanner(System.in);
		encodedText = scanner.nextLine();
		encodedText = encodedText.replaceAll("/", " ");
		try {
			String[] encodedSplitWords = encodedText.split(" ");
			int[] encodedSplitWordsLength = new int[encodedSplitWords.length];
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < encodedSplitWords.length; i++) {
				encodedSplitWordsLength[i] = encodedSplitWords[i].length();
			}
			for (int i = 0; i < encodedSplitWordsLength.length; i += 2) {
				if (encodedSplitWordsLength[i] == 0) {
					builder.append(' ');
					i--;
					continue;
				}
				int x = encodedSplitWordsLength[i] - 1;
				int y = encodedSplitWordsLength[i + 1] - 1;
				builder.append(keyMatrix[x][y]);
			}
			decodedText = builder.toString();
			System.out.println(decodedText);
		} catch (Exception e) {
			System.out.println("Try again.Something went wrong! " + e);
		}
	}
}

