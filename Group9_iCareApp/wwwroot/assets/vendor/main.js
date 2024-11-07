import {
	ClassicEditor,
	AccessibilityHelp,
	Autoformat,
	AutoImage,
	Autosave,
	BlockQuote,
	Bold,
	CKBox,
	CKBoxImageEdit,
	CloudServices,
	Essentials,
	Heading,
	HtmlComment,
	HtmlEmbed,
	ImageBlock,
	ImageCaption,
	ImageInline,
	ImageInsert,
	ImageInsertViaUrl,
	ImageResize,
	ImageStyle,
	ImageTextAlternative,
	ImageToolbar,
	ImageUpload,
	Indent,
	IndentBlock,
	Italic,
	Link,
	LinkImage,
	List,
	ListProperties,
	Paragraph,
	PictureEditing,
	SelectAll,
	ShowBlocks,
	SourceEditing,
	Table,
	TableCaption,
	TableCellProperties,
	TableColumnResize,
	TableProperties,
	TableToolbar,
	TextTransformation,
	TodoList,
	Underline,
	Undo
} from 'ckeditor5';
import { ExportPdf } from 'ckeditor5-premium-features';

/**
 * Please update the following values with your actual tokens.
 * Instructions on how to obtain them: https://ckeditor.com/docs/trial/latest/guides/real-time/quick-start.html
 */
const LICENSE_KEY = 'aWVURFhGSDE0d1VEeW1ic0t3aEN6QURLbWRUeFc2c2p3VWlqOGhtN3dWYklCSFFVb2h4czFaRFJYL3JFcVE9PS1NakF5TkRFeU1EVT0=';
const CKBOX_TOKEN_URL = 'https://122298.cke-cs.com/token/dev/f71e2d49b0248b1923f48dc2540edcaf4e8caaa3be344acf9a4a5b57628e?limit=10';

const editorConfig = {
	toolbar: {
		items: [
			'undo',
			'redo',
			'|',
			'sourceEditing',
			'exportPdf',
			'showBlocks',
			'|',
			'heading',
			'|',
			'bold',
			'italic',
			'underline',
			'|',
			'link',
			'insertImage',
			'ckbox',
			'insertTable',
			'blockQuote',
			'htmlEmbed',
			'|',
			'bulletedList',
			'numberedList',
			'todoList',
			'outdent',
			'indent'
		],
		shouldNotGroupWhenFull: false
	},
	plugins: [
		AccessibilityHelp,
		Autoformat,
		AutoImage,
		Autosave,
		BlockQuote,
		Bold,
		CKBox,
		CKBoxImageEdit,
		CloudServices,
		Essentials,
		ExportPdf,
		Heading,
		HtmlComment,
		HtmlEmbed,
		ImageBlock,
		ImageCaption,
		ImageInline,
		ImageInsert,
		ImageInsertViaUrl,
		ImageResize,
		ImageStyle,
		ImageTextAlternative,
		ImageToolbar,
		ImageUpload,
		Indent,
		IndentBlock,
		Italic,
		Link,
		LinkImage,
		List,
		ListProperties,
		Paragraph,
		PictureEditing,
		SelectAll,
		ShowBlocks,
		SourceEditing,
		Table,
		TableCaption,
		TableCellProperties,
		TableColumnResize,
		TableProperties,
		TableToolbar,
		TextTransformation,
		TodoList,
		Underline,
		Undo
	],
	ckbox: {
		tokenUrl: CKBOX_TOKEN_URL
	},
	exportPdf: {
		stylesheets: [
			/* This path should point to application stylesheets. */
			/* See: https://ckeditor.com/docs/ckeditor5/latest/features/converters/export-pdf.html */
			'./style.css',
			/* Export PDF needs access to stylesheets that style the content. */
			'./ckeditor5/ckeditor5.css',
			'./ckeditor5-premium-features/ckeditor5-premium-features.css'
		],
		fileName: 'file.pdf',
		converterOptions: {
			format: 'Tabloid',
			margin_top: '20mm',
			margin_bottom: '20mm',
			margin_right: '24mm',
			margin_left: '24mm',
			page_orientation: 'portrait'
		}
	},
	heading: {
		options: [
			{
				model: 'paragraph',
				title: 'Paragraph',
				class: 'ck-heading_paragraph'
			},
			{
				model: 'heading1',
				view: 'h1',
				title: 'Heading 1',
				class: 'ck-heading_heading1'
			},
			{
				model: 'heading2',
				view: 'h2',
				title: 'Heading 2',
				class: 'ck-heading_heading2'
			},
			{
				model: 'heading3',
				view: 'h3',
				title: 'Heading 3',
				class: 'ck-heading_heading3'
			},
			{
				model: 'heading4',
				view: 'h4',
				title: 'Heading 4',
				class: 'ck-heading_heading4'
			},
			{
				model: 'heading5',
				view: 'h5',
				title: 'Heading 5',
				class: 'ck-heading_heading5'
			},
			{
				model: 'heading6',
				view: 'h6',
				title: 'Heading 6',
				class: 'ck-heading_heading6'
			}
		]
	},
	image: {
		toolbar: [
			'toggleImageCaption',
			'imageTextAlternative',
			'|',
			'imageStyle:inline',
			'imageStyle:wrapText',
			'imageStyle:breakText',
			'|',
			'resizeImage',
			'|',
		]
	},
	link: {
		addTargetToExternalLinks: true,
		defaultProtocol: 'https://',
		decorators: {
			toggleDownloadable: {
				mode: 'manual',
				label: 'Downloadable',
				attributes: {
					download: 'file'
				}
			}
		}
	},
	list: {
		properties: {
			styles: true,
			startIndex: true,
			reversed: true
		}
	},
	placeholder: 'Type or paste your content here!',
	table: {
		contentToolbar: ['tableColumn', 'tableRow', 'mergeTableCells', 'tableProperties', 'tableCellProperties']
	},
	typing: {
		transformations: {
			extra: [{ from: 'ace', to: 'Acetaminophen' },
				{ from: 'add', to: 'Adderall' },
				{ from: 'amo', to: 'Amoxicillin' },
				{ from: 'dox', to: 'Doxycycline' },
				{ from: 'ibu', to: 'Ibuprofen' },
				{ from: 'cyc', to: 'Cyclobenzaprine' },
				{ from: 'oze', to: 'Ozempic' },
				{ from: 'nar', to: 'Narcan' },
				{ from: 'nal', to: 'Naltrexone' },
				{ from: 'mel', to: 'Melatonin' },
				{ from: 'via', to: 'Viagra' },
				{ from: 'xan', to: 'Xanax' }],
		}
	}
};

configUpdateAlert(editorConfig);
let editor;
ClassicEditor.create(document.querySelector('#editor'), editorConfig).then(newEditor => {
	editor = newEditor;
});


document.querySelector('#submit').addEventListener('click', () => {
	const editorData = editor.getData();

	if (editorData) {
		console.log('Editor Data:', editorData);
		fetch('/ManageDocument/SaveDocument', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json' // Specify JSON format
			},
			body: JSON.stringify({ content: editorData }) // Send editorData as JSON
		})
			.then(response => {
				if (response.ok) {
					return response.json();
				}
				throw new Error('Network response was not ok.');
			})
			.then(data => {
				console.log('Server response:', data);
				// Optionally handle response data here, such as updating the UI or showing a success message
			})
			.catch(error => {
				console.error('Error:', error);
			});
		// Here you could send editorData to your ASP.NET MVC controller or process it as needed
	} else {
		console.error('Editor instance not available.');
	}

	// ...
});

/**
 * This function exists to remind you to update the config needed for premium features.
 * The function can be safely removed. Make sure to also remove call to this function when doing so.
 */
function configUpdateAlert(config) {
	if (configUpdateAlert.configUpdateAlertShown) {
		return;
	}

	const isModifiedByUser = (currentValue, forbiddenValue) => {
		if (currentValue === forbiddenValue) {
			return false;
		}

		if (currentValue === undefined) {
			return false;
		}

		return true;
	};

	const valuesToUpdate = [];

	configUpdateAlert.configUpdateAlertShown = true;

	if (!isModifiedByUser(config.ckbox?.tokenUrl, '<YOUR_CKBOX_TOKEN_URL>')) {
		valuesToUpdate.push('CKBOX_TOKEN_URL');
	}

	if (valuesToUpdate.length) {
		window.alert(
			[
				'Please update the following values in your editor config',
				'in order to receive full access to the Premium Features:',
				'',
				...valuesToUpdate.map(value => ` - ${value}`)
			].join('\n')
		);
	}
}
